using Home_Assistant_Desktop.Services;
using Home_Assistant_Desktop.ValueObjects;
using System.Runtime.InteropServices;

namespace Home_Assistant_Desktop
{
    public partial class UpdateAvailableForm : Form
    {
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWCP_ROUND = 2;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int pvAttribute, int cbAttribute);

        private readonly bool updateChecksEnabled;

        // Original designer-set layout, captured once so every state transition computes
        // an absolute target instead of a relative shift from whatever the current
        // (possibly already-resized) state happens to be.
        private readonly int fullClientHeight;
        private readonly int originalDownloadTop;
        private readonly int originalDontNotifyTop;

        public UpdateAvailableAction SelectedAction { get; private set; } = UpdateAvailableAction.Dismissed;

        /// <summary>
        ///  Shows an already-known result immediately (e.g. from a balloon tip click,
        ///  where a background check already resolved the version).
        /// </summary>
        public UpdateAvailableForm(Version? latestVersion, Version currentVersion, bool updateChecksEnabled)
        {
            InitializeComponent();

            this.updateChecksEnabled = updateChecksEnabled;
            fullClientHeight = ClientSize.Height;
            originalDownloadTop = buttonDownload.Top;
            originalDontNotifyTop = buttonDontNotify.Top;

            ApplyResult(latestVersion, currentVersion);

            pictureLogo.Image = AppLogo.Load();
        }

        /// <summary>
        ///  Shows a "Checking for Updates" loading state immediately, then updates itself
        ///  once pendingCheck resolves - so a slow or dead connection doesn't leave the
        ///  user staring at a closed tray menu with no feedback at all.
        /// </summary>
        public UpdateAvailableForm(Task<UpdateCheckResult> pendingCheck, Version currentVersion, bool updateChecksEnabled)
        {
            InitializeComponent();

            this.updateChecksEnabled = updateChecksEnabled;
            fullClientHeight = ClientSize.Height;
            originalDownloadTop = buttonDownload.Top;
            originalDontNotifyTop = buttonDontNotify.Top;

            ShowCheckingState();

            pictureLogo.Image = AppLogo.Load();

            _ = AwaitPendingCheckAsync(pendingCheck, currentVersion);
        }

        private void ShowCheckingState()
        {
            labelTitle.Text = "Checking for Updates";
            labelMessage.Text = "Checking for the latest version, please wait...";

            buttonDownload.Visible = false;
            buttonSkip.Visible = false;
            buttonDontNotify.Visible = false;

            this.ClientSize = new Size(this.ClientSize.Width, labelMessage.Bottom + 20);
        }

        private async Task AwaitPendingCheckAsync(Task<UpdateCheckResult> pendingCheck, Version currentVersion)
        {
            UpdateCheckResult result = await pendingCheck;

            if (IsDisposed)
                return;

            ApplyResult(result.LatestVersion, currentVersion);
            CenterHeaderRow();
            CenterOnPrimaryScreen();
        }

        private void ApplyResult(Version? latestVersion, Version currentVersion)
        {
            bool updateAvailable = latestVersion is not null && latestVersion > currentVersion;

            labelTitle.Text = updateAvailable ? "Update Available" : "You're Up to Date";
            labelMessage.Text = updateAvailable
                ? $"Version {latestVersion} is available (you have {currentVersion})."
                : $"You're already running the latest version ({currentVersion}).";

            buttonDontNotify.Text = updateChecksEnabled ? "Don't notify me for Updates" : "Notify me for Updates";
            buttonDontNotify.Visible = true;

            buttonDownload.Visible = updateAvailable;
            buttonSkip.Visible = updateAvailable;

            if (updateAvailable)
            {
                buttonDontNotify.Top = originalDontNotifyTop;
                this.ClientSize = new Size(this.ClientSize.Width, fullClientHeight);
            }
            else
            {
                buttonDontNotify.Top = originalDownloadTop;
                this.ClientSize = new Size(this.ClientSize.Width, fullClientHeight - (originalDontNotifyTop - originalDownloadTop));
            }
        }

        private void UpdateAvailableForm_Load(object sender, EventArgs e)
        {
            int cornerPreference = DWMWCP_ROUND;
            DwmSetWindowAttribute(Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref cornerPreference, sizeof(int));

            CenterHeaderRow();
            CenterOnPrimaryScreen();
        }

        private void CenterHeaderRow()
        {
            const int gap = 10;

            Size textSize = TextRenderer.MeasureText(labelTitle.Text, labelTitle.Font);
            int rowWidth = pictureLogo.Width + gap + textSize.Width;
            int startX = (this.ClientSize.Width - rowWidth) / 2;

            pictureLogo.Location = new Point(startX, pictureLogo.Top);
            labelTitle.Location = new Point(startX + pictureLogo.Width + gap, labelTitle.Top);
            labelTitle.Width = textSize.Width;
        }

        private void CenterOnPrimaryScreen()
        {
            Rectangle screenBounds = Screen.PrimaryScreen?.Bounds ?? Screen.AllScreens[0].Bounds;

            this.Location = new Point(
                screenBounds.Left + (screenBounds.Width - this.Width) / 2,
                screenBounds.Top + (screenBounds.Height - this.Height) / 2);
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            SelectedAction = UpdateAvailableAction.Download;
            Close();
        }

        private void buttonSkip_Click(object sender, EventArgs e)
        {
            SelectedAction = UpdateAvailableAction.SkipThisRelease;
            Close();
        }

        private void buttonDontNotify_Click(object sender, EventArgs e)
        {
            SelectedAction = updateChecksEnabled
                ? UpdateAvailableAction.DontNotifyAgain
                : UpdateAvailableAction.EnableNotifications;
            Close();
        }

        private void UpdateAvailableForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void AnyControl_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UpdateAvailableForm_Deactivate(object sender, EventArgs e)
        {
            Close();
        }
    }
}
