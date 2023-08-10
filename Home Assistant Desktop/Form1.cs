using Home_Assistant_Desktop.Properties;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Home_Assistant_Desktop
{
    public partial class Form1 : Form
    {
        string currentViewPosition = "bottomRight";
        Size currentViewSize = new Size(441, 811);
        string currentStartURL = "http://homeassistant.local:8123";
        Boolean currentStayOnTop = true;

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
                var nccsp = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));

                nccsp.rgrc0.top += 1;
                nccsp.rgrc0.bottom -= 8;
                nccsp.rgrc0.left += 8;
                nccsp.rgrc0.right -= 8;

                Marshal.StructureToPtr(nccsp, m.LParam, true);
            }
            else
            {
                var clnRect = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));

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
            setViewPosition("topLeft");
        }

        private void itemAlignViewTopRight_Click(object sender, EventArgs e)
        {
            setViewPosition("topRight");
        }

        private void itemAlignViewBottomLeft_Click(object sender, EventArgs e)
        {
            setViewPosition("bottomLeft");
        }

        private void itemAlignViewBottomRight_Click(object sender, EventArgs e)
        {
            setViewPosition("bottomRight");
        }

        private void itemSetStartURL_Click(object sender, EventArgs e)
        {
            string startURL = Interaction.InputBox("Enter Start URL:", "Set Start URL", currentStartURL).Trim();

            if (Uri.IsWellFormedUriString(startURL, UriKind.Absolute) && startURL != "")
            {
                setStartURL(startURL);
            }
            else if (startURL == "")
            {
                return;
            }
            else
            {
                MessageBox.Show("Invalid URL Entered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                itemSetStartURL.PerformClick();
            }
        }

        private void itemStayOnTop_Click(object sender, EventArgs e)
        {
            setStayOnTop(!currentStayOnTop);
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

        private void itemQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void resizeWebView()
        {
            mainWebView.Size = new Size(this.Width + 1, this.Height);
        }


        private void setViewPosition(string position)
        {
            switch (position)
            {
                case "topLeft":
                    this.Location = new Point(0, 0);
                    currentViewPosition = position;
                    break;

                case "topRight":
                    this.Location = new Point(Screen.FromHandle(this.Handle).WorkingArea.Width - this.Width, 0);
                    currentViewPosition = position;
                    break;

                case "bottomLeft":
                    this.Location = new Point(0, Screen.FromHandle(this.Handle).WorkingArea.Height - this.Height);
                    currentViewPosition = position;
                    break;

                case "bottomRight":
                    this.Location = new Point(Screen.FromHandle(this.Handle).WorkingArea.Width - this.Width, Screen.FromHandle(this.Handle).WorkingArea.Height - this.Height);
                    currentViewPosition = position;
                    break;

                default:
                    setViewPosition("bottomRight");
                    break;
            }

            RenderViewItemsChanges();
        }

        private void setStayOnTop(Boolean set)
        {
            this.TopMost = set;
            currentStayOnTop = set;

            RenderViewItemsChanges();
        }

        private void setStartURL(string url)
        {
            currentStartURL = url;
            mainWebView.Source = new Uri(url);
        }

        private void setViewSize(Size size)
        {
            currentViewSize = size;
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
            catch (Exception e)
            {

            }
        }

        private void RenderViewItemsChanges()
        {
            itemAlignViewTopLeft.Text = "Align View Top Left";
            itemAlignViewTopRight.Text = "Align View Top Right";
            itemAlignViewBottomLeft.Text = "Align View Bottom Left";
            itemAlignViewBottomRight.Text = "Align View Bottom Right";

            if (currentViewPosition == "topLeft")
                itemAlignViewTopLeft.Text = "✓ Align View Top Left";

            if (currentViewPosition == "topRight")
                itemAlignViewTopRight.Text = "✓ Align View Top Right";

            if (currentViewPosition == "bottomLeft")
                itemAlignViewBottomLeft.Text = "✓ Align View Bottom Left";

            if (currentViewPosition == "bottomRight")
                itemAlignViewBottomRight.Text = "✓ Align View Bottom Right";

            itemStayOnTop.Text = "Stay on Top";

            if (currentStayOnTop == true)
                itemStayOnTop.Text = "✓ Stay on Top";

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
            Settings.Default.savedStartURL = currentStartURL;
            Settings.Default.savedViewPosition = currentViewPosition;
            Settings.Default.savedViewSize = currentViewSize;
            Settings.Default.savedStayOnTop = currentStayOnTop;

            Properties.Settings.Default.Save();
        }

        private void LoadCurrentSettings()
        {
            if (Settings.Default.savedStartURL != "")
            {
                setStartURL(Settings.Default.savedStartURL);
            }

            if (Settings.Default.savedViewSize != new Size(0, 0))
            {
                setViewSize(Settings.Default.savedViewSize);
            }

            if (Settings.Default.savedViewPosition != "")
            {
                setViewPosition(Settings.Default.savedViewPosition);
            }

            setStayOnTop(Settings.Default.savedStayOnTop);

            resizeWebView();
        }

        private void ResetSettings()
        {
            Settings.Default.Reset();
            MessageBox.Show("Application has been reset.", "Application Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();
        }
    }
}