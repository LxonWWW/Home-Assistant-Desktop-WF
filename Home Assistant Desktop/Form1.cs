using Home_Assistant_Desktop.Data;
using Home_Assistant_Desktop.Factories;
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCurrentSettings();

            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "";

            showView(false);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            setViewSize(this.Size);
            resizeWebView();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            showView(false);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                showView(true);
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

        private void showView(Boolean set)
        {
            try
            {
                this.Visible = set;

                if (set == true)
                {
                    this.Opacity = 100;
                    this.Focus();
                    this.Activate();
                }
                else
                {
                    this.Opacity = 0;
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

            showView(true);
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