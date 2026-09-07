namespace Home_Assistant_Desktop.ValueObjects
{
    /// <summary>
    ///  Immutable snapshot of the window's view configuration. Use `with` expressions to derive changes.
    /// </summary>
    public sealed record AppViewState(Uri StartUrl, ViewPosition Position, Size WindowSize, bool StayOnTop, bool RememberLastPage)
    {
        public static readonly Uri DefaultStartUrl = new("http://homeassistant.local:8123");
        public static readonly Size DefaultWindowSize = new(441, 811);
        public const ViewPosition DefaultPosition = ViewPosition.BottomRight;
        public const bool DefaultStayOnTop = false;
        public const bool DefaultRememberLastPage = false;

        public static AppViewState CreateDefault() =>
            new(DefaultStartUrl, DefaultPosition, DefaultWindowSize, DefaultStayOnTop, DefaultRememberLastPage);
    }
}
