using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Home_Assistant_Desktop
{
    public partial class AboutForm : Form
    {
        private const string RepositoryUrl = "https://github.com/LxonWWW/Home-Assistant-Desktop-WF";

        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWCP_ROUND = 2;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int pvAttribute, int cbAttribute);

        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            int cornerPreference = DWMWCP_ROUND;
            DwmSetWindowAttribute(Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref cornerPreference, sizeof(int));

            pictureLogo.Image = LoadLogo();

            Version? version = Assembly.GetExecutingAssembly().GetName().Version;
            labelVersion.Text = version is null ? "Version unknown" : $"Version {version.Major}.{version.Minor}.{version.Build}";

            CenterOnPrimaryScreen();
        }

        private static Image? LoadLogo()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using Stream? logoStream = assembly.GetManifestResourceStream("Home_Assistant_Desktop.home-assistant-icon.png");

            if (logoStream is not null)
                return Image.FromStream(logoStream);

            using Icon? appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            return appIcon?.ToBitmap();
        }

        private void CenterOnPrimaryScreen()
        {
            Rectangle screenBounds = Screen.PrimaryScreen?.Bounds ?? Screen.AllScreens[0].Bounds;

            this.Location = new Point(
                screenBounds.Left + (screenBounds.Width - this.Width) / 2,
                screenBounds.Top + (screenBounds.Height - this.Height) / 2);
        }

        private void linkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process browser = new Process();
            browser.StartInfo.UseShellExecute = true;
            browser.StartInfo.FileName = RepositoryUrl;
            browser.Start();
        }

        private void AnyControl_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutForm_Deactivate(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
