using System;
using System.Collections.Generic;
using System.Linq;
using BugsXNA.Behaviors;
using BugsXNA.Common;
using BugsXNA.Common.DebugTools;
using BugsXNA.Common.GameStateManagement;
using BugsXNA.Models;
using BugsXNA.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using BugsXNA.Controllers;

namespace BugsXNA
{
    public class BugsGame : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;
        private readonly Controller _controller;
        private DebugSystem _debugSystem;

        public BugsGame()
        {
            BugsGame.Instance = this;

            //Instansiate and setup graphics device manager
            _graphics = new GraphicsDeviceManager(this)
                           {
                               IsFullScreen = true,
                               PreferredBackBufferHeight = 480,
                               PreferredBackBufferWidth = 800,
                               SupportedOrientations = DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft
                           };

            // Create the screen factory and add it to the Services
            Services.AddService(typeof(IScreenFactory), new ScreenFactory());

            // Create the screen manager component.
            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            _controller = new Controller(_screenManager);

            //Set Content root directory
            Content.RootDirectory = "Content";

            _debugSystem = DebugSystem.Initialize(this, "MenuFont");

            // Frame rate is 30 fps by default for Windows Phone
            // The original "WeAreBugs" runs at 60 fps - so we do that too in XNA
            TargetElapsedTime = TimeSpan.FromSeconds(1f / 60f);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _controller.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _debugSystem.TimeRuler.StartFrame();
            _debugSystem.TimeRuler.BeginMark("Update", Color.Blue);
            _debugSystem.FpsCounter.Visible = true;
            _debugSystem.TimeRuler.Visible = true;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            _controller.Update(gameTime);

            base.Update(gameTime);

            _debugSystem.TimeRuler.EndMark("Update");
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _debugSystem.TimeRuler.BeginMark("Draw", Color.Yellow);

            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);

            _debugSystem.TimeRuler.EndMark("Draw");
        }

        public SpriteBatch SpriteBatch { get { return _screenManager.SpriteBatch; } }
        public static BugsGame Instance { get; private set; }
    }
}
