using Home_Assistant_Desktop.Properties;
using Home_Assistant_Desktop.ValueObjects;

namespace Home_Assistant_Desktop.Data
{
    /// <summary>
    ///  Data access object isolating reads/writes of the generated ApplicationSettingsBase
    ///  store behind the AppViewState value object.
    /// </summary>
    public sealed class AppSettingsRepository
    {
        public AppViewState Load()
        {
            Settings settings = Settings.Default;

            Uri startUrl = Uri.TryCreate(settings.savedStartURL, UriKind.Absolute, out Uri? parsedUrl)
                ? parsedUrl
                : AppViewState.DefaultStartUrl;

            Size windowSize = settings.savedViewSize != Size.Empty
                ? settings.savedViewSize
                : AppViewState.DefaultWindowSize;

            ViewPosition position = ViewPositionConvert.FromSettingsValue(settings.savedViewPosition);
            TrayInteractionMode interactionMode = TrayInteractionModeConvert.FromSettingsValue(settings.savedTrayInteractionMode);

            return new AppViewState(startUrl, position, windowSize, settings.savedStayOnTop, settings.savedRememberLastPage, interactionMode);
        }

        public void Save(AppViewState state)
        {
            Settings settings = Settings.Default;

            settings.savedStartURL = state.StartUrl.ToString();
            settings.savedViewPosition = state.Position.ToSettingsValue();
            settings.savedViewSize = state.WindowSize;
            settings.savedStayOnTop = state.StayOnTop;
            settings.savedRememberLastPage = state.RememberLastPage;
            settings.savedTrayInteractionMode = state.InteractionMode.ToSettingsValue();

            settings.Save();
        }

        public void Reset() => Settings.Default.Reset();
    }
}
