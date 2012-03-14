using System;
using System.Collections.Generic;
using System.Linq;
using BugsXNA.Behaviors;
using BugsXNA.Common;
using BugsXNA.Common.GameStateManagement;
using BugsXNA.Controllers;
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

namespace BugsXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BugsGame : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly ScreenManager screenManager;

        public BugsGame()
        {
            BugsGame.Instance = this;

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

            // Create the screen factory and add it to the Services
            Services.AddService(typeof(IScreenFactory), new ScreenFactory());

            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            // Hook events on the PhoneApplicationService so we're notified of the application's life cycle
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Launching += new EventHandler<Microsoft.Phone.Shell.LaunchingEventArgs>(GameLaunching);
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Activated += new EventHandler<Microsoft.Phone.Shell.ActivatedEventArgs>(GameActivated);
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Deactivated += new EventHandler<Microsoft.Phone.Shell.DeactivatedEventArgs>(GameDeactivated);
            
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //_controller.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        private void AddStartScreen()
        {
            // We have different menus for Windows Phone to take advantage of the touch interface
            screenManager.AddScreen(new StartScreen(), null);
        }

        private void GameLaunching(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
            AddStartScreen();
        }

        private void GameActivated(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
        {
            // Try to deserialize the screen manager
            if (!screenManager.Activate(e.IsApplicationInstancePreserved))
            {
                // If the screen manager fails to deserialize, add the initial screens
                AddStartScreen();
            }
        }

        private void GameDeactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            // Serialize the screen manager when the game deactivated
            screenManager.Deactivate();
        }

        public SpriteBatch SpriteBatch { get { return screenManager.SpriteBatch; } }
        public static BugsGame Instance { get; private set; }
    }
}
