using Microsoft.Xna.Framework;

namespace GameProject {
    static class Time {
        public static float Delta { get; private set; }
        public static float Total { get; private set; }
        public static double DeltaFull { get; private set; }
        public static double TotalFull { get; private set; }

        internal static void Update(GameTime gameTime) {
            Delta = (float)(DeltaFull = gameTime.ElapsedGameTime.TotalSeconds);
            Total = (float)(TotalFull = gameTime.TotalGameTime.TotalSeconds);
        }
    }
}