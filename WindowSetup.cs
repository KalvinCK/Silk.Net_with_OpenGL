using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace MyGame
{
    static class WindowSetup
    {
        public static WindowOptions options;
        static WindowSetup()
        {
            options = WindowOptions.Default;

            options.Title = "SILK Net App";
            options.Size = new Vector2D<int>(1280, 720);
            options.Position = new Vector2D<int>(100, 100);
            options.VideoMode = VideoMode.Default;
            options.WindowBorder = WindowBorder.Resizable;

        }
    }
}