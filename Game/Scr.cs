using Microsoft.Xna.Framework.Graphics;

namespace GameProject {
    abstract class Screen {
        public abstract void LoadContent();
        public abstract void ExitScreen();
        public abstract void Update();
        public abstract void Draw(SpriteBatch s);
    }
}