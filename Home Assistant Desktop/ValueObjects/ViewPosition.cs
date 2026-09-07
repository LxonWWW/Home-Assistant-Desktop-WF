namespace Home_Assistant_Desktop.ValueObjects
{
    public enum ViewPosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public static class ViewPositionConvert
    {
        public static string ToSettingsValue(this ViewPosition position) => position switch
        {
            ViewPosition.TopLeft => "topLeft",
            ViewPosition.TopRight => "topRight",
            ViewPosition.BottomLeft => "bottomLeft",
            ViewPosition.BottomRight => "bottomRight",
            _ => "bottomRight"
        };

        public static ViewPosition FromSettingsValue(string? value) => value switch
        {
            "topLeft" => ViewPosition.TopLeft,
            "topRight" => ViewPosition.TopRight,
            "bottomLeft" => ViewPosition.BottomLeft,
            "bottomRight" => ViewPosition.BottomRight,
            _ => ViewPosition.BottomRight
        };
    }
}
