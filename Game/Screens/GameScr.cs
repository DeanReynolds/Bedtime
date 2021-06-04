using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GameProject {
    class GameScr : Screen {
        public override void Draw(SpriteBatch s) {
            throw new System.NotImplementedException();
        }

        public override void ExitScreen() {
            throw new System.NotImplementedException();
        }

        public override void LoadContent() {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Main.Content.Load<Song>("Bedtime_Game_Music1"));
        }

        public override void Update() {
            throw new System.NotImplementedException();
        }
    }
}