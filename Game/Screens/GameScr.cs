using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GameProject {
    class GameScr : Screen {
        public override void Draw(SpriteBatch s) {}

        public override void ExitScreen() {}

        public override void LoadContent() {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Main.Content.Load<Song>("Bedtime_Game_Music1"));
        }

        public override void Update() {}
    }
}