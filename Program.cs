using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using Silk.NET.Core;

using System.Numerics;

namespace MyGame
{
    static class Program
    {
        public static IWindow window { get; private set; } = Window.Create(WindowSetup.options);
        private static GL ?_gl;    
        private static IKeyboard ?_keyboard;
        public static GL Gl { get => _gl!; }
        public static IKeyboard Keyboard { get => _keyboard!; }
        public static Vector2D<int> Size { get => window.Size; }
        private static Camera Camera = new Camera(Vector3.UnitZ * 6);
        private static Game ?setup;
        private static unsafe void Main()
        {
            

            window.Load += delegate
            {
                IInputContext input = window.CreateInput();
                _keyboard = input.Keyboards.FirstOrDefault()!;

                bool FullScreen = false, vsync = false;

                if (_keyboard != null)
                {
                    _keyboard.KeyDown += delegate(IKeyboard keyboard, Key key, int arg3)
                    {
                        if(key == Key.Escape)
                        {
                            window.Close();
                        }
                        if(key == Key.F)
                        {
                            FullScreen = FullScreen ? false : true;
                            window.WindowState = FullScreen ? WindowState.Fullscreen : WindowState.Normal;
                        }
                        if(key == Key.V)
                        {
                            vsync = vsync ? false : true;
                            window.VSync = vsync;
                        }
                    };
                }
                for (int i = 0; i < input.Mice.Count; i++)
                {
                    input.Mice[i].Cursor.CursorMode = CursorMode.Raw;

                    input.Mice[i].MouseMove += delegate(IMouse mouse, Vector2 CursorPosition)
                    {
                        Camera.UpdateDirectionView(CursorPosition);
                    };

                    input.Mice[i].Scroll += delegate(IMouse mouse, ScrollWheel scrollWheel)
                    {
                        Camera.UpdateFov(scrollWheel.Y);
                    };
                }
                _gl = GL.GetApi(window);
                _gl.ClearColor(System.Drawing.Color.Black);
                _gl.Enable(EnableCap.DepthTest);

                setup = new Game();
            };
            window.Render += delegate(double deltaTime)
            {
                setup!.Render();
            };


            window.Update += delegate(double deltaTime)
            {
                Clock.TimerUpdateFrame(deltaTime);

                Console.WriteLine(Clock.FramesPerSecond);

                Camera.UpdatePosition();

                setup!.Update();
            };
            window.Resize += delegate(Vector2D<int> size)
            {
                Gl.Viewport(size);
                Camera.ResizedRatio(size);
                setup!.Resize();

            };
            window.Closing += delegate
            {
                setup!.Dispose();
            };

            window.Run();
        }
    }
}