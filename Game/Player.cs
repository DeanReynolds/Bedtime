using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject {
    static class Player {
        static Vector2 _position;
        public static Rectangle Hitbox { get; private set; }

        public static void Update() {
            Hitbox = new Rectangle((int)_position.X - 20, (int)_position.Y - 20, 40, 40);
        }

        public static void Draw(SpriteBatch s) {
            s.FillRectangle(Hitbox, Color.White * .5f);
        }
    }
}