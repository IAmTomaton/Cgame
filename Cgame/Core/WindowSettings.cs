using OpenTK;

namespace Cgame.Core
{
    class WindowSettings
    {
        public int Width { get; }
        public int Height { get; }
        public string Title { get; }

        public WindowSettings(int width, int height, string title)
        {
            Width = width;
            Height = height;
            Title = title;
        }
    }
}
