using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameProject {
    class GameScr : Screen {
        static RenderTarget2D _mirror;
        public static RenderTarget2D ScreenTarget { get; private set; }
        public static RenderTarget2D UITarget { get; private set; }
        public static Rectangle[] SolidHitboxes { get; private set; }

        static float _sanity, _teethBrushed, _light, _lightOffTimer,
        _lightTarget, _lightFadeSpeed, _sanityVisibility;
        static bool _brushingTeeth, _isLightOff;
        static int _lightFlickers;
        static readonly Rng Rng = new Rng();

        public override void Draw(SpriteBatch s) {
            Main.GraphicsDevice.SetRenderTarget(_mirror);
            s.Begin();
            const float mirrorToWorldRatio = 160f / 39f;
            s.Draw(Main.Content.Load<Texture2D>("background"), new Vector2(0, 0), null, Color.White,
                0, Vector2.Zero, 1, 0, 0);
            s.FillRectangle(new Vector2(((Player.Position.X - 29) * mirrorToWorldRatio) - ((Player.Hitbox.Width / 2) * mirrorToWorldRatio),
                    45 - Player.Position.Y + 34), new Vector2(Player.Hitbox.Width, Player.Hitbox.Height) * mirrorToWorldRatio, Color.Blue * .75f, 0,
                origin : new Vector2(Player.Hitbox.Width * .5f, Player.Hitbox.Height * .5f));
            s.End();
            Main.GraphicsDevice.SetRenderTarget(ScreenTarget);
            s.Begin();
            s.Draw(Main.Content.Load<Texture2D>("background"), Vector2.Zero, Color.White);
            s.Draw(Main.Content.Load<Texture2D>("door"), Vector2.Zero, Color.White);
            s.Draw(Main.Content.Load<Texture2D>("sink"), Vector2.Zero, Color.White);
            s.Draw(Main.Content.Load<Texture2D>("mirror"), Vector2.Zero, Color.White);
            s.Draw(Main.Content.Load<Texture2D>("toilet"), Vector2.Zero, Color.White);
            if (_light == 0)
                s.Draw(Main.Content.Load<Texture2D>("switchoff"), Vector2.Zero, Color.White);
            else
                s.Draw(Main.Content.Load<Texture2D>("switchon"), Vector2.Zero, Color.White);
            s.Draw(_mirror, new Rectangle(31, 8, 39, 15), Color.White);
            s.FillRectangle(new Vector2(Player.Position.X, Player.Position.Y), new Vector2(Player.Hitbox.Width, Player.Hitbox.Height), Color.Blue * .75f, 0,
                origin : new Vector2(Player.Hitbox.Width * .5f, Player.Hitbox.Height * .5f));
            if (_light < 1)
                s.FillRectangle(new Rectangle(0, 0, ScreenTarget.Width, ScreenTarget.Height), Color.Black * .666f * (1 - _light));
            s.End();
            Main.GraphicsDevice.SetRenderTarget(UITarget);
            s.Begin();
            Main.GraphicsDevice.Clear(Color.Transparent);
            if (_sanityVisibility > 0) {
                Rectangle sanityBar = new Rectangle(340, 510, 280, 20);
                var sanityUIOpacity = MathF.Min(_sanityVisibility * 3, 1);
                s.FillRectangle(sanityBar, Color.DarkGray * sanityUIOpacity);
                s.FillRectangle(new Rectangle(sanityBar.X, sanityBar.Y, (int)(sanityBar.Width * _sanity), sanityBar.Height), Color.Lerp(Color.DarkRed, Color.LimeGreen * sanityUIOpacity, _sanity));
                s.DrawRectangle(sanityBar, Color.Black * sanityUIOpacity, RectStyle.Outline, thickness : 4);
                s.Draw(Main.Content.Load<Texture2D>("sanityText"), new Vector2(480, 490), null, Color.White * sanityUIOpacity,
                    0, new Vector2(137.5f, 28.5f), MathF.Max(.4f, .3f + (MathF.Cos(Time.Total * 3) * .25f)), 0, 0);
            }
            Rectangle teethBrushedBar = new Rectangle(340, 50, 280, 20);
            s.FillRectangle(teethBrushedBar, Color.DarkGray);
            s.FillRectangle(new Rectangle(teethBrushedBar.X, teethBrushedBar.Y, (int)(teethBrushedBar.Width * _teethBrushed), teethBrushedBar.Height), Color.Lerp(Color.Yellow, Color.White, _teethBrushed));
            s.DrawRectangle(teethBrushedBar, Color.Black, RectStyle.Outline, thickness : 4);
            s.Draw(Main.Content.Load<Texture2D>("teethBrushedText"), new Vector2(480, 30), null, Color.White,
                0, new Vector2(294.5f, 27), .3f, 0, 0);
            s.End();
            Main.GraphicsDevice.SetRenderTarget(null);
            s.Begin(samplerState: SamplerState.PointClamp);
            s.Draw(ScreenTarget, new Rectangle(0, 0, Main.Window.ClientBounds.Width, Main.Window.ClientBounds.Height), Color.White);
            s.Draw(UITarget, new Rectangle(0, 0, Main.Window.ClientBounds.Width, Main.Window.ClientBounds.Height), Color.White);
            s.End();
        }

        public override void ExitScreen() {}

        public override void LoadContent() {
            ScreenTarget = new RenderTarget2D(Main.GraphicsDevice, 160, 90);
            _mirror = new RenderTarget2D(Main.GraphicsDevice, 160, 90);
            UITarget = new RenderTarget2D(Main.GraphicsDevice, 960, 540);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Main.Content.Load<Song>("Bedtime_Game_Music1"));
            SolidHitboxes = new [] {
                new Rectangle(0, 0, 160, 34)
            };
            Player.Position = new Vector2(80, 79);
            _sanity = 1;
            _teethBrushed = 0;
            _brushingTeeth = false;
            _lightTarget = _light = 1;
            _lightOffTimer = 0;
            _sanityVisibility = _lightFlickers = 0;
            Main.Content.Load<SoundEffect>("flipswitch");
        }

        public override void Update() {
            if (!_brushingTeeth)
                Player.Update();
            else {
                _teethBrushed += Time.Delta * .01f;
                if (_teethBrushed > 1)
                    _teethBrushed = 1;
            }
            if (Input.AnyKeyPressed(Keys.E)) {
                if (_brushingTeeth)
                    _brushingTeeth = false;
                else if (Player.Position.Y <= 42 && Player.Position.X > 34 && Player.Position.X < 76)
                    _brushingTeeth = true;
                if (((_light == 0 && _isLightOff) || _light == 1) && Player.Position.Y <= 42 && Player.Position.X > 90 && Player.Position.X < 104) {
                    _lightTarget = _light = 1 - _light;
                    if (_light == 0) {
                        _isLightOff = true;
                        MediaPlayer.Play(Main.Content.Load<Song>("Bedtime_Game_Music2"));
                    } else {
                        _isLightOff = false;
                        MediaPlayer.Play(Main.Content.Load<Song>("Bedtime_Game_Music1"));
                    }
                    Main.Content.Load<SoundEffect>("flipswitch").Play();
                }
            }
            if (_light == 0) {
                if (_sanity > 0) {
                    _sanity -= Time.Delta * .1f;
                    if (_sanity < 0)
                        _sanity = 0;
                }
            } else {
                if (_sanity < 1) {
                    _sanity += Time.Delta * .1f;
                    if (_sanity > 1)
                        _sanity = 1;
                }
            }
            if (_sanity < 1) {
                if ((_sanityVisibility += Time.Delta * .1f) > 1)
                    _sanityVisibility = 1;
            } else if ((_sanityVisibility -= Time.Delta * .5f) < 0)
                _sanityVisibility = 0;
            if (_light != _lightTarget)
                _light += MathF.Sign(_lightTarget - _light) * _lightFadeSpeed * Time.Delta;
            if (MathF.Abs(_light - _lightTarget) <= .01f && !_isLightOff) {
                _light = _lightTarget;
                if (_lightFlickers > 0) {
                    if (Rng.NextFloat() <= .1f) {
                        _lightTarget = _light = 0;
                        _isLightOff = true;
                        MediaPlayer.Play(Main.Content.Load<Song>("Bedtime_Game_Music2"));
                        Main.Content.Load<SoundEffect>("flipswitch").Play();
                    } else if (_lightTarget != 1) {
                        _lightTarget = 1;
                        _lightFadeSpeed = Rng.NextFloat(.4f, 1.2f);
                    } else {
                        _lightFlickers--;
                        if (_lightFlickers > 0) {
                            _lightTarget = Rng.NextFloat(.3f, .9f);
                            _lightFadeSpeed = Rng.NextFloat(.4f, 1.2f);
                        }
                    }
                } else if (_light == 1) {
                    _lightOffTimer += Time.Delta;
                    if (_lightOffTimer >= 3) {
                        if (Rng.NextFloat() <= .2f) {
                            _lightFlickers += Rng.Next(1, 5);
                            _lightTarget = Rng.NextFloat(.3f, .9f);
                            _lightFadeSpeed = Rng.NextFloat(.4f, 1.2f);
                        }
                        _lightOffTimer -= 3;
                    }
                }
            }
        }
    }
}