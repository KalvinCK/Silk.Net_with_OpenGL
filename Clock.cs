

namespace MyGame
{
    public struct Clock
    {
        public static float Time                { get; private set; } = 0.0f;
        public static float ElapsedTime         { get; private set; } = 0.0f;
        public static float FramesPerSecond     { get; private set; } = 0.0f;
        
        private static double previousTime = 0.0, frameCount = 0.0;
        public static void TimerUpdateFrame(double deltaTime)
        {
            ElapsedTime = (float)deltaTime;
            Time += (float)deltaTime;

            frameCount++;
            if(Time - previousTime >= 1.0)
            {
                FramesPerSecond = (float)frameCount;
                frameCount = 0;
                previousTime = Time;
            }
        }
    }
}