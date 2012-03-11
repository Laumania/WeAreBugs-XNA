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
        private readonly GraphicsDeviceManager graphics;
        private Controller _controller;

        //private float _seekPointRampDistance = 50; //distance at which the bug will start slowing its approach.

        public BugsGame()
        {
            BugsGame.Instance = this; 
            
            //Instansiate and setup graphics device manager
            graphics = new GraphicsDeviceManager(this)
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

        //public BugModel BugModel { get; set; }
        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _controller = new Controller(this);
            _controller.Initialize();



            //_enemyList = new List<AgentModel>();
            //TargetPoint = Vector2.Zero;

            //BugModel = new BugModel(this);
            //BugModel.Add(new SeekPointBehavior(() => this.GetTarget(), _seekPointRampDistance));


            //Components.Add(BugModel);
            //Components.Add(CreateEnemyModel());
            //Components.Add(CreateEnemyModel());
            //Components.Add(CreateEnemyModel());

            base.Initialize();
        }

        //public void UpdateEnemies(GameTime gameTime)
        //{
        //    bool bugIsCaught = false;
        //    foreach (EnemyModel enemyModel in _enemyList)
        //    {
        //        float distance = Mathematics.Distance(enemyModel.Position, BugModel.Position);
        //        enemyModel.Excitement = Math.Max(0, (_enemyExcitementThreshold - distance) / _enemyExcitementThreshold);
        //        enemyModel.Update(gameTime);
        //        if (distance < 15)
        //        {
        //            bugIsCaught = true;
        //        }
        //    }
        //}

        //public Vector2 GetTarget()
        //{
        //    return TargetPoint;
        //}

        //private List<AgentModel> _enemyList;
        //private float _foodThreshold = 20; //disttance at which the bug is considered to have eaten the food.
        //private float _foodPlacementBorderWidth = 100; //defines box where food can be placed.
        //private float _enemyCreationBufferWidth = 20;
        //private float _enemyAtBugThreshold = 20; //distance at which the enemy is considered to have caught the bug.
        //private float _enemyPredictionFactor = 0f; //distance the enemy will "lead" the bug while pursuing.
        //private float _enemySeperationFactor = 15f; //minimum distance between enemies.
        //private float _enemyMaxSpeedLowRange = 1.5f; //low end value for random MaxSpeed
        //private float _enemyMaxSpeedHighRange = 5.7f; //high end value for random MaxSpeed
        //private float _enemyMaxForceLowRange = .01f; //low end value for random MaxForce
        //private float _enemyMaxForceHighRange = .032f; //high end value for random MaxForce
        //private float _enemyExcitementThreshold = 150; //the distance from the bug at which the enemy becomes excited (turns color)
        //private Random _rnd = new Random();

        //private EnemyModel CreateEnemyModel()
        //{
        //    EnemyModel enemyModel = new EnemyModel(this);
        //    enemyModel.Front = new Vector2(0, -1);
        //    enemyModel.Mass = .2f;
        //    enemyModel.Scale = Mathematics.Lerp(.4f, .8f, (float)_rnd.NextDouble());

        //    enemyModel.MaxSpeed = (float)Mathematics.Lerp(_enemyMaxSpeedLowRange, _enemyMaxSpeedHighRange, (float)_rnd.NextDouble());
        //    enemyModel.MaxForce = (float)Mathematics.Lerp(_enemyMaxForceLowRange, _enemyMaxForceHighRange, (float)_rnd.NextDouble());

        //    int quadrant = _rnd.Next(1, 5);
        //    switch (quadrant)
        //    {
        //        case 1:
        //            enemyModel.Position = new Vector2((float)_rnd.NextDouble() * Width, -_enemyCreationBufferWidth);
        //            break;
        //        case 2:
        //            enemyModel.Position = new Vector2(Width + _enemyCreationBufferWidth, (float)_rnd.NextDouble() * Height);
        //            break;
        //        case 3:
        //            enemyModel.Position = new Vector2((float)_rnd.NextDouble() * Width, Width + _enemyCreationBufferWidth);
        //            break;
        //        default:
        //            enemyModel.Position = new Vector2(-_enemyCreationBufferWidth, (float)_rnd.NextDouble() * Height);
        //            break;
        //    }
        //    enemyModel.Initialize();
        //    enemyModel.Add(new PursueAgentBehavior(_enemyAtBugThreshold, _enemyPredictionFactor, BugModel));
        //    enemyModel.Add(new SeperationBehavior(_enemySeperationFactor, _enemyList));
        //    return enemyModel;
        //}

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
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

        
            //UpdateEnemies(gameTime);

            base.Update(gameTime);
        }

        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);
            base.Draw(gameTime);
        }

        public SpriteBatch SpriteBatch { get; private set; }
        //public Vector2 TargetPoint { get; set; }
        public int Width { get { return graphics.PreferredBackBufferWidth; } }
        public int Height { get { return graphics.PreferredBackBufferHeight; } }

        public static BugsGame Instance { get; private set; }
    }
}
