using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;

namespace MyGame
{
    public class Camera
    {
        private float Yaw { get; set; } = -90f;
        private float Pitch { get; set; }

        //---------------------------------------------
        public static Vector3 Position { get; private set; }
        public static Vector3 Front { get; private set; } = Vector3.UnitZ * -1;
        public static Vector3 Up { get; private set; } = Vector3.UnitY;
        public static Vector3 Right 
        {
            get => Vector3.Normalize(Vector3.Cross(Front, Up));
        }
        public static Matrix4x4 ViewMatrix
        {
            get => Matrix4x4.CreateLookAt(Position, Position + Front, Up);
        }
        public static Matrix4x4 ProjectionMatrix
        {
            get => Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), AspectRatio, 0.1f, 1000.0f);
        }

        private static float Fov = 90f;
        public static float AspectRatio { get; set; } = Program.window.Size.X / (float)Program.window.Size.Y;

        public Camera(Vector3 position)
        {
            Position = position;
        }
        public const float lookSensitivity = 0.2f;
        private const float moveSpeed = 2.5f; 

        private Vector2 LastMousePosition;
        public void UpdateDirectionView(Vector2 cursorPosition)
        {
            
            if (LastMousePosition == default) 
            { 
                LastMousePosition = cursorPosition; 
            }
            else
            {
                var xOffset = (cursorPosition.X - LastMousePosition.X) * lookSensitivity;
                var yOffset = (cursorPosition.Y - LastMousePosition.Y) * lookSensitivity;
                LastMousePosition = cursorPosition;

                
                Yaw += xOffset;
                Pitch -= yOffset;

                Pitch = Math.Clamp(Pitch, -89f, 89f);

                var cameraDirection = Vector3.Zero;
                cameraDirection.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
                cameraDirection.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
                cameraDirection.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));

                Front = Vector3.Normalize(cameraDirection);
            }
        }
        public void UpdatePosition()
        {
            var input = Program.Keyboard.IsKeyPressed;
            
            if (input(Key.W)) Position +=  Front * moveSpeed * Clock.ElapsedTime;
            if (input(Key.S)) Position -=  Front * moveSpeed * Clock.ElapsedTime;
            if (input(Key.A)) Position -=  Right * moveSpeed * Clock.ElapsedTime;
            if (input(Key.D)) Position +=  Right * moveSpeed * Clock.ElapsedTime;


            if(input(Key.Space)) Position += Up * moveSpeed * Clock.ElapsedTime;
            if(input(Key.C))     Position -= Up * moveSpeed * Clock.ElapsedTime;

            if(input(Key.ShiftLeft) && input(Key.W)) Position += Front * (moveSpeed * 10.0f) * Clock.ElapsedTime;
            if(input(Key.ShiftLeft) && input(Key.S)) Position -= Front * (moveSpeed * 10.0f) * Clock.ElapsedTime;
        }
        public void UpdateFov(float zoomAmount)
        {
            Fov = Math.Clamp(Fov - zoomAmount, 1.0f, 45f);
        }
        public void ResizedRatio(Vector2D<int> size)
        {
            AspectRatio = size.X / (float)size.Y;
        }
    }
}