using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GameProject {
    class GameScr : Screen {
        static RenderTarget2D _mirror;
        public static RenderTarget2D ScreenTarget { get; private set; }
        public static Rectangle[] SolidHitboxes { get; private set; }

        public override void Draw(SpriteBatch s) {
            Main.GraphicsDevice.SetRenderTarget(ScreenTarget);
            s.Begin(samplerState: SamplerState.PointClamp);
            s.Draw(Main.Content.Load<Texture2D>("background"), Vector2.Zero, Color.White);
            Player.Draw(s);
            s.End();
            Main.GraphicsDevice.SetRenderTarget(null);
            s.Begin(samplerState: SamplerState.PointClamp);
            s.Draw(ScreenTarget, new Rectangle(0, 0, Main.Window.ClientBounds.Width, Main.Window.ClientBounds.Height), Color.White);
            s.End();
        }

        public override void ExitScreen() {}

        public override void LoadContent() {
            ScreenTarget = new RenderTarget2D(Main.GraphicsDevice, 160, 90);
            _mirror = new RenderTarget2D(Main.GraphicsDevice, 160, 90);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Main.Content.Load<Song>("Bedtime_Game_Music1"));
            SolidHitboxes = new [] {
                new Rectangle(0, 0, 160, 34)
            };
            Player.Position = new Vector2(80, 79);
        }

        public override void Update() {
            Player.Update();
        }
    }
}