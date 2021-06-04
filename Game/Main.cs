using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject {
    class Main : Game {
        static readonly Dictionary<Type, Screen> _scr = new Dictionary<Type, Screen>();
        public static GraphicsDeviceManager Graphics { get; private set; }
        public static SpriteBatch S { get; private set; }
        public static new ContentManager Content { get; private set; }
        public static Screen Screen { get; private set; }

        public static void SetScreen<T>()where T : Screen {
            Screen.ExitScreen();
            Screen = _scr[typeof(T)];
            Screen.LoadContent();
        }

        readonly ICondition _quit =
            new AnyCondition(
                new KeyboardCondition(Keys.Escape),
                new GamePadCondition(GamePadButton.Back, 0)
            );

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
            S = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            InputHelper.Setup(this);

            Screen = _scr[typeof(GameScr)];
            Screen.LoadContent();
        }

        protected override void Update(GameTime gameTime) {
            InputHelper.UpdateSetup();

            if (_quit.Pressed())
                Exit();

            // TODO: Add your update logic here

            InputHelper.UpdateCleanup();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}