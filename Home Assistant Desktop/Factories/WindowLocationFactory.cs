using Home_Assistant_Desktop.ValueObjects;

namespace Home_Assistant_Desktop.Factories
{
    /// <summary>
    ///  Computes the on-screen Point a window should be moved to for a given ViewPosition.
    /// </summary>
    public static class WindowLocationFactory
    {
        public static Point Create(ViewPosition position, Screen screen, Size windowSize)
        {
            Rectangle workingArea = screen.WorkingArea;

            return position switch
            {
                ViewPosition.TopLeft => new Point(0, 0),
                ViewPosition.TopRight => new Point(workingArea.Width - windowSize.Width, 0),
                ViewPosition.BottomLeft => new Point(0, workingArea.Height - windowSize.Height),
                ViewPosition.BottomRight => new Point(workingArea.Width - windowSize.Width, workingArea.Height - windowSize.Height),
                _ => Create(ViewPosition.BottomRight, screen, windowSize)
            };
        }
    }
}
