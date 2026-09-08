using Home_Assistant_Desktop.Data;
using Home_Assistant_Desktop.Factories;
using Home_Assistant_Desktop.Services;
using Home_Assistant_Desktop.ValueObjects;
using Microsoft.VisualBasic;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Home_Assistant_Desktop
{
    public partial class Form1 : Form
    {
        private readonly AppSettingsRepository settingsRepository = new();
        private AppViewState viewState = AppViewState.CreateDefault();

        private bool hoverWatchLatched = false;
        private Point iconHoverOrigin;
        private const int IconHoverZoneMargin = 24;

        private readonly GitHubUpdateChecker updateChecker = new();
        private static readonly Random random = new();
        private static readonly TimeSpan UpdateCheckBaseInterval = TimeSpan.FromMinutes(60);
        private static readonly TimeSpan UpdateCheckJitter = TimeSpan.FromMinutes(15);
        private static readonly TimeSpan InitialUpdateCheckDelay = TimeSpan.FromMinutes(2);
        private const string ReleasesUrl = "https://github.com/LxonWWW/Home-Assistant-Desktop-WF/releases/latest";
        private Version? pendingUpdateVersion;
        private Task<UpdateCheckResult>? inFlightUpdateCheck;

        // START Windows API for Window Resizing while having no borders

        public const uint WM_NCPAINT = 0x85;
        public const uint WM_NCCALCSIZE = 0x83;
        public const uint WM_NCHITTEST = 0x84;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left, top, right, bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndinsertafter;
            public int x, y, cx, cy;
            public int flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NCCALCSIZE_PARAMS
        {
            public RECT rgrc0, rgrc1, rgrc2;
            public WINDOWPOS lppos;
        }

        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
                base.WndProc(ref m);

            switch ((uint)m.Msg)
            {
                case WM_NCCALCSIZE: WmNCCalcSize(ref m); break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void WmNCCalcSize(ref Message m)
        {
            if (m.WParam != IntPtr.Zero)
            {
                var nccsp = Marshal.PtrToStructure<NCCALCSIZE_PARAMS>(m.LParam);

                nccsp.rgrc0.top += 1;
                nccsp.rgrc0.bottom -= 8;
                nccsp.rgrc0.left += 8;
                nccsp.rgrc0.right -= 8;

                Marshal.StructureToPtr(nccsp, m.LParam, true);
            }
            else
            {
                var clnRect = Marshal.PtrToStructure<RECT>(m.LParam);

                clnRect.top += 0;
                clnRect.bottom -= 8;
                clnRect.left += 8;
                clnRect.right -= 8;

                Marshal.StructureToPtr(clnRect, m.LParam, true);
            }

            m.Result = IntPtr.Zero;
        }

        // END Windows API for Window Resizing while having no borders

        public Form1()
        {
            InitializeComponent();

            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
        }

        private void SystemEvents_DisplaySettingsChanged(object? sender, EventArgs e)
        {
            // SystemEvents raises this on its own worker thread if the subscribing
            // thread (here, before Application.Run starts pumping messages) wasn't
            // already pumping - marshal back before touching any control.
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => SystemEvents_DisplaySettingsChanged(sender, e)));
                return;
            }

            // Re-apply the saved alignment directly: Windows can snap the window to a
            // fallback position when a monitor briefly disappears (e.g. during
            // sleep/resume with an external display reconnecting). Reposition only -
            // setViewPosition would also force the hidden tray popup to show/focus.
            this.Location = WindowLocationFactory.Create(viewState.Position, Screen.FromHandle(this.Handle), this.Size);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCurrentSettings();

            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "";
            this.DoubleBuffered = true;

            ScheduleInitialUpdateCheck();

            showView(false);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            setViewSize(this.Size);
            resizeWebView();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (viewState.InteractionMode != TrayInteractionMode.OpenOnToggle)
                showView(false);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return;

            if (viewState.InteractionMode == TrayInteractionMode.OpenOnHover)
            {
                showView(false);
            }
            else
            {
                showView(!this.Visible, true);
            }
        }

        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e)
        {
            if (viewState.InteractionMode == TrayInteractionMode.OpenOnHover && !contextMenuStrip1.Visible && !this.Visible)
            {
                iconHoverOrigin = Cursor.Position;
                showView(true);
            }
        }

        private void hoverWatchTimer_Tick(object sender, EventArgs e)
        {
            int margin = (int)(IconHoverZoneMargin * (this.DeviceDpi / 96f));
            Rectangle iconZone = new(iconHoverOrigin.X - margin, iconHoverOrigin.Y - margin, margin * 2, margin * 2);

            if (!this.Bounds.Contains(Cursor.Position) && !iconZone.Contains(Cursor.Position))
            {
                hoverWatchTimer.Enabled = false;
                showView(false);
            }
        }


        private void itemOpenInBrowser_Click(object sender, EventArgs e)
        {
            openBrowser();
        }

        private void itemAlignViewTopLeft_Click(object sender, EventArgs e)
        {
            setViewPosition(ViewPosition.TopLeft);
        }

        private void itemAlignViewTopRight_Click(object sender, EventArgs e)
        {
            setViewPosition(ViewPosition.TopRight);
        }

        private void itemAlignViewBottomLeft_Click(object sender, EventArgs e)
        {
            setViewPosition(ViewPosition.BottomLeft);
        }

        private void itemAlignViewBottomRight_Click(object sender, EventArgs e)
        {
            setViewPosition(ViewPosition.BottomRight);
        }

        private void itemSetStartURL_Click(object sender, EventArgs e)
        {
            string startURL = Interaction.InputBox("Enter Start URL:", "Set Start URL", viewState.StartUrl.ToString()).Trim();

            if (startURL == "")
            {
                return;
            }
            else if (Uri.TryCreate(startURL, UriKind.Absolute, out Uri? parsedUrl))
            {
                setStartURL(parsedUrl);
            }
            else
            {
                MessageBox.Show("Invalid URL Entered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                itemSetStartURL.PerformClick();
            }
        }

        private void itemStayOnTop_Click(object sender, EventArgs e)
        {
            setStayOnTop(!viewState.StayOnTop);
        }

        private void itemRememberLastPage_Click(object sender, EventArgs e)
        {
            setRememberLastPage(!viewState.RememberLastPage);
        }

        private void itemOpenOnHover_Click(object sender, EventArgs e)
        {
            setInteractionMode(viewState.InteractionMode == TrayInteractionMode.OpenOnHover
                ? TrayInteractionMode.ClickToOpen
                : TrayInteractionMode.OpenOnHover);
        }

        private void itemOpenOnToggle_Click(object sender, EventArgs e)
        {
            setInteractionMode(viewState.InteractionMode == TrayInteractionMode.OpenOnToggle
                ? TrayInteractionMode.ClickToOpen
                : TrayInteractionMode.OpenOnToggle);
        }

        private void mainWebView_SourceChanged(object? sender, CoreWebView2SourceChangedEventArgs e)
        {
            if (!viewState.RememberLastPage || mainWebView.Source is null)
                return;

            viewState = viewState with { StartUrl = mainWebView.Source };
            SaveCurrentSettings();
        }

        private void itemRestartApplication_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void itemResetApplication_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete all saved settings?", "Reset Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                ResetSettings();
        }

        private void itemSaveCurrentSettings_Click(object sender, EventArgs e)
        {
            SaveCurrentSettings();
            MessageBox.Show("Saved current settings successfully.", "Settings Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void itemAbout_Click(object sender, EventArgs e)
        {
            using AboutForm aboutForm = new();
            aboutForm.ShowDialog();
        }

        private async void itemCheckForUpdates_Click(object sender, EventArgs e)
        {
            Version currentVersion = AppVersion.Current;
            Task<UpdateCheckResult> checkTask = StartOrJoinUpdateCheck();

            using UpdateAvailableForm dialog = new(checkTask, currentVersion, viewState.UpdateChecksEnabled);
            dialog.ShowDialog();

            UpdateCheckResult result = await checkTask;
            ApplyDialogAction(dialog.SelectedAction, result.LatestVersion);
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            if (pendingUpdateVersion is null)
                return;

            using UpdateAvailableForm dialog = new(pendingUpdateVersion, AppVersion.Current, viewState.UpdateChecksEnabled);
            dialog.ShowDialog();

            ApplyDialogAction(dialog.SelectedAction, pendingUpdateVersion);
        }

        private async void updateCheckTimer_Tick(object sender, EventArgs e)
        {
            updateCheckTimer.Stop();

            if (!viewState.UpdateChecksEnabled)
                return;

            UpdateCheckResult result = await StartOrJoinUpdateCheck();
            Version currentVersion = AppVersion.Current;

            if (result.LatestVersion is null || result.LatestVersion <= currentVersion)
                return;

            Version? lastNotified = settingsRepository.GetLastNotifiedUpdateVersion();
            if (lastNotified is not null && lastNotified >= result.LatestVersion)
                return;

            pendingUpdateVersion = result.LatestVersion;
            notifyIcon1.BalloonTipTitle = "Update Available";
            notifyIcon1.BalloonTipText = $"Version {result.LatestVersion} is available. Click to view.";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.ShowBalloonTip(10000);
        }

        private void itemQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void resizeWebView()
        {
            mainWebView.Size = new Size(this.Width + 1, this.Height);
        }


        private void setViewPosition(ViewPosition position)
        {
            this.Location = WindowLocationFactory.Create(position, Screen.FromHandle(this.Handle), this.Size);
            viewState = viewState with { Position = position };

            RenderViewItemsChanges();
        }

        private void setStayOnTop(Boolean set)
        {
            this.TopMost = set;
            viewState = viewState with { StayOnTop = set };

            RenderViewItemsChanges();
        }

        private void setRememberLastPage(Boolean set)
        {
            viewState = viewState with { RememberLastPage = set };

            if (set && mainWebView.Source is not null)
            {
                viewState = viewState with { StartUrl = mainWebView.Source };
                SaveCurrentSettings();
            }

            RenderViewItemsChanges();
        }

        private void setInteractionMode(TrayInteractionMode mode)
        {
            viewState = viewState with { InteractionMode = mode };

            RenderViewItemsChanges();
        }

        private void setUpdateChecksEnabled(bool enabled)
        {
            viewState = viewState with { UpdateChecksEnabled = enabled };
            SaveCurrentSettings();
        }

        private static void OpenReleasesPage()
        {
            Process browser = new Process();
            browser.StartInfo.UseShellExecute = true;
            browser.StartInfo.FileName = ReleasesUrl;
            browser.Start();
        }

        private static TimeSpan GetJitteredUpdateCheckInterval()
        {
            double offsetMinutes = (random.NextDouble() * 2 - 1) * UpdateCheckJitter.TotalMinutes;
            return UpdateCheckBaseInterval + TimeSpan.FromMinutes(offsetMinutes);
        }

        private void ScheduleTimerFor(DateTime nextCheckAtUtc)
        {
            TimeSpan delay = nextCheckAtUtc - DateTime.UtcNow;
            if (delay < TimeSpan.Zero)
                delay = TimeSpan.FromSeconds(5);

            updateCheckTimer.Stop();
            updateCheckTimer.Interval = (int)Math.Min(delay.TotalMilliseconds, int.MaxValue - 1);
            updateCheckTimer.Start();
        }

        private void ScheduleInitialUpdateCheck()
        {
            if (!viewState.UpdateChecksEnabled)
                return;

            DateTime dueAt = settingsRepository.GetNextUpdateCheckAt() ?? DateTime.UtcNow;

            if (dueAt <= DateTime.UtcNow)
                dueAt = DateTime.UtcNow + InitialUpdateCheckDelay;

            ScheduleTimerFor(dueAt);
        }

        private Task<UpdateCheckResult> StartOrJoinUpdateCheck()
        {
            inFlightUpdateCheck ??= RunUpdateCheckAsync();
            return inFlightUpdateCheck;
        }

        private async Task<UpdateCheckResult> RunUpdateCheckAsync()
        {
            try
            {
                UpdateCheckResult result = await updateChecker.CheckAsync();
                ApplyCheckSchedule(result);
                return result;
            }
            finally
            {
                inFlightUpdateCheck = null;
            }
        }

        private void ApplyCheckSchedule(UpdateCheckResult result)
        {
            if (!viewState.UpdateChecksEnabled)
                return;

            DateTime nextCheckAt = result.RetryNotBefore?.UtcDateTime ?? DateTime.UtcNow + GetJitteredUpdateCheckInterval();
            settingsRepository.SetNextUpdateCheckAt(nextCheckAt);
            ScheduleTimerFor(nextCheckAt);
        }

        private void ApplyDialogAction(UpdateAvailableAction action, Version? latestVersion)
        {
            switch (action)
            {
                case UpdateAvailableAction.Download:
                    OpenReleasesPage();
                    break;

                case UpdateAvailableAction.SkipThisRelease:
                    if (latestVersion is not null)
                        settingsRepository.SetLastNotifiedUpdateVersion(latestVersion);
                    break;

                case UpdateAvailableAction.DontNotifyAgain:
                    setUpdateChecksEnabled(false);
                    break;

                case UpdateAvailableAction.EnableNotifications:
                    setUpdateChecksEnabled(true);
                    ScheduleInitialUpdateCheck();
                    break;
            }
        }

        private void setStartURL(Uri url)
        {
            viewState = viewState with { StartUrl = url };
            mainWebView.Source = url;
        }

        private void setViewSize(Size size)
        {
            viewState = viewState with { WindowSize = size };
            this.Size = size;
        }

        private void showView(Boolean set, Boolean openedExplicitly = false)
        {
            try
            {
                this.Visible = set;

                if (!hoverWatchLatched)
                    hoverWatchLatched = openedExplicitly;

                bool watchForHoverExit = viewState.InteractionMode == TrayInteractionMode.OpenOnHover && set && !hoverWatchLatched;

                hoverWatchTimer.Enabled = watchForHoverExit;

                if (set == true)
                {
                    this.Opacity = 100;
                    this.Focus();
                    this.Activate();
                }
                else
                {
                    this.Opacity = 0;
                    hoverWatchLatched = false;
                }
            }
            catch (Exception)
            {
                // Form may already be disposed during application shutdown.
            }
        }

        private void RenderViewItemsChanges()
        {
            itemAlignViewTopLeft.Text = "Align View Top Left";
            itemAlignViewTopRight.Text = "Align View Top Right";
            itemAlignViewBottomLeft.Text = "Align View Bottom Left";
            itemAlignViewBottomRight.Text = "Align View Bottom Right";

            if (viewState.Position == ViewPosition.TopLeft)
                itemAlignViewTopLeft.Text = "✓ Align View Top Left";

            if (viewState.Position == ViewPosition.TopRight)
                itemAlignViewTopRight.Text = "✓ Align View Top Right";

            if (viewState.Position == ViewPosition.BottomLeft)
                itemAlignViewBottomLeft.Text = "✓ Align View Bottom Left";

            if (viewState.Position == ViewPosition.BottomRight)
                itemAlignViewBottomRight.Text = "✓ Align View Bottom Right";

            itemStayOnTop.Text = "Stay on Top";

            if (viewState.StayOnTop == true)
                itemStayOnTop.Text = "✓ Stay on Top";

            itemRememberLastPage.Text = "Remember Last Page";

            if (viewState.RememberLastPage == true)
                itemRememberLastPage.Text = "✓ Remember Last Page";

            itemOpenOnHover.Text = "Open on Hover";

            if (viewState.InteractionMode == TrayInteractionMode.OpenOnHover)
                itemOpenOnHover.Text = "✓ Open on Hover";

            itemOpenOnToggle.Text = "Open on Toggle";

            if (viewState.InteractionMode == TrayInteractionMode.OpenOnToggle)
                itemOpenOnToggle.Text = "✓ Open on Toggle";

            showView(true, true);
        }

        private void openBrowser()
        {
            Process browser = new Process();
            browser.StartInfo.UseShellExecute = true;
            browser.StartInfo.FileName = mainWebView.Source.ToString();
            browser.Start();
        }

        private void SaveCurrentSettings()
        {
            settingsRepository.Save(viewState);
        }

        private void LoadCurrentSettings()
        {
            viewState = settingsRepository.Load();

            setStartURL(viewState.StartUrl);
            setViewSize(viewState.WindowSize);
            setViewPosition(viewState.Position);
            setStayOnTop(viewState.StayOnTop);
            setRememberLastPage(viewState.RememberLastPage);
            setInteractionMode(viewState.InteractionMode);

            resizeWebView();
        }

        private void ResetSettings()
        {
            settingsRepository.Reset();
            MessageBox.Show("Application has been reset.", "Application Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();
        }
    }
}