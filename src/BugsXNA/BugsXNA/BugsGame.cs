using System;
using System.Collections.Generic;
using System.Linq;
using BugsXNA.Behaviors;
using BugsXNA.Common;
using BugsXNA.Controllers;
using BugsXNA.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace BugsXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BugsGame : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly Controller _controller;
        private Texture2D _backgroundTexture;

        public BugsGame()
        {
            BugsGame.Instance = this;
            _controller = new Controller(this);

            //Setup touch capabilities
            TouchPanel.EnabledGestures = GestureType.Tap; 
            
            //Instansiate and setup graphics device manager
            _graphics = new GraphicsDeviceManager(this)
                           {
                               IsFullScreen = true,
                               PreferredBackBufferHeight = 480,
                               PreferredBackBufferWidth = 800,
                               SupportedOrientations = DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft
                           };

            //Set Content root directory
            Content.RootDirectory = "Content";

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
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            _backgroundTexture = this.Content.Load<Texture2D>("Background");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            if (SpriteBatch != null) SpriteBatch.Dispose();
            SpriteBatch = null;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            _controller.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            DrawBackground();
            base.Draw(gameTime);
        }

        private void DrawBackground()
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, 800, 480), Color.White);
            SpriteBatch.End();
        }

        public SpriteBatch SpriteBatch { get; private set; }
        public int Width { get { return _graphics.PreferredBackBufferWidth; } }
        public int Height { get { return _graphics.PreferredBackBufferHeight; } }

        public static BugsGame Instance { get; private set; }
    }
}
