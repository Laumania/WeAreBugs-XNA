using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using BugsXNA.Behaviors;
using BugsXNA.Common;
using BugsXNA.Common.GameStateManagement;
using BugsXNA.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BugsXNA.Screens
{
    public class StartScreen : GameScreen
    {
        private GameModel _gameModel;
        private ContentManager _content;
        private SpriteFont _font;
        private float _seekPointRampDistance = 50; //distance at which the bug will start slowing its approach.
        private float _foodThreshold = 20; //disttance at which the bug is considered to have eaten the food.

        public StartScreen()
        {
            // Create a button to start the game
            //Button playButton = new Button("Play");
            //playButton.Tapped += playButton_Tapped;
            //MenuButtons.Add(playButton);
        }

        public override void Activate(bool instancePreserved)
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _font = _content.Load<SpriteFont>("MenuFont");

            _gameModel = new GameModel(ScreenManager.Game);
            _gameModel.Width = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            _gameModel.Height = ScreenManager.Game.GraphicsDevice.Viewport.Height;
            _gameModel.Initialize();
            _gameModel.Start();

            _gameModel.BugModel.Add(new SeekPointBehavior(() => _gameModel.GetTarget(), _seekPointRampDistance));

            base.Activate(instancePreserved);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _gameModel.TargetPoint = _gameModel.FoodModel.Position;
            if (Mathematics.Distance(_gameModel.BugModel.Position, _gameModel.FoodModel.Position) < _foodThreshold)
            {
                _gameModel.SetFood();
            }
            _gameModel.Update(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(_font, "'To a good approximation, all species are insects'", Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.End();
        }

        void playButton_Tapped(object sender, EventArgs e)
        {
            // When the "Play" button is tapped, we load the GameplayScreen
            //LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new GameplayScreen());
        }
    }
}
