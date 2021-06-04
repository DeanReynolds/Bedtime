using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject {
    class Main : Game {
        static readonly Dictionary<Type, Screen> _scr = new Dictionary<Type, Screen>();
        public static GraphicsDeviceManager Graphics { get; private set; }
        public static new GraphicsDevice GraphicsDevice { get; private set; }
        public static new GameWindow Window { get; private set; }
        public static SpriteBatch S { get; private set; }
        public static new ContentManager Content { get; private set; }
        public static Screen Screen { get; private set; }

        public static void SetScreen<T>()where T : Screen {
            Screen.ExitScreen();
            Screen = _scr[typeof(T)];
            Screen.LoadContent();
        }

        public Main() {
            Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content = base.Content;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            foreach (var s in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(Screen)))) {
                var t = (Screen)Activator.CreateInstance(s);
                // s.GetMethod("Init").Invoke(t, null);
                _scr.Add(s, t);
            }
            base.Initialize();
        }

        protected override void LoadContent() {
            SpriteBatchExtensions.Init();

            Window = base.Window;
            GraphicsDevice = base.GraphicsDevice;
            S = new SpriteBatch(GraphicsDevice);

            Screen = _scr[typeof(GameScr)];
            Screen.LoadContent();
        }

        protected override void Update(GameTime gameTime) {
            Input.Update();

            if (Input.AnyKeyPressed(Keys.Escape))
                Exit();

            Screen.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            Screen.Draw(S);

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args) {
            Screen.ExitScreen();
            base.OnExiting(sender, args);
        }
    }
}