using System.Reflection;

namespace Home_Assistant_Desktop
{
    internal static class AppVersion
    {
        /// <summary>
        ///  Assembly.GetName().Version is always 4-part (e.g. 1.3.9.0); this trims the
        ///  trailing revision so it matches GitHub's 3-part tag format (1.3.9) and
        ///  displays cleanly, consistently, everywhere the app's version is shown.
        /// </summary>
        public static Version Current
        {
            get
            {
                Version? assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
                return assemblyVersion is null
                    ? new Version(0, 0, 0)
                    : new Version(assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build);
            }
        }
    }
}
