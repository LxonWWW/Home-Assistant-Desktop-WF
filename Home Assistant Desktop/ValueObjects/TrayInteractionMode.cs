namespace Home_Assistant_Desktop.ValueObjects
{
    public enum TrayInteractionMode
    {
        ClickToOpen,
        OpenOnHover,
        OpenOnToggle
    }

    public static class TrayInteractionModeConvert
    {
        public static string ToSettingsValue(this TrayInteractionMode mode) => mode switch
        {
            TrayInteractionMode.OpenOnHover => "openOnHover",
            TrayInteractionMode.OpenOnToggle => "openOnToggle",
            _ => "clickToOpen"
        };

        public static TrayInteractionMode FromSettingsValue(string? value) => value switch
        {
            "openOnHover" => TrayInteractionMode.OpenOnHover,
            "openOnToggle" => TrayInteractionMode.OpenOnToggle,
            _ => TrayInteractionMode.ClickToOpen
        };
    }
}
