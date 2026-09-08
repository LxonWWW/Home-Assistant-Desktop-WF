using System.Reflection;

namespace Home_Assistant_Desktop
{
    internal static class AppLogo
    {
        public static Image? Load()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using Stream? logoStream = assembly.GetManifestResourceStream("Home_Assistant_Desktop.home-assistant-icon.png");

            if (logoStream is not null)
                return Image.FromStream(logoStream);

            using Icon? appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            return appIcon?.ToBitmap();
        }
    }
}
