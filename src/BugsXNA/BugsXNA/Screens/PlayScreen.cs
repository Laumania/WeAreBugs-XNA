using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugsXNA.Behaviors;
using BugsXNA.Common.GameStateManagement;
using BugsXNA.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BugsXNA.Screens
{
    public class PlayScreen : BaseScreen
    {
        private Texture2D _background;
        private GameModel _gameModel;
        private ContentManager _content;
        private SpriteFont _font;
        private float _seekPointRampDistance = 50; //distance at which the bug will start slowing its approach.

        public PlayScreen(GameModel gameModel)
        {
            TransitionOffTime = TimeSpan.Zero;
            TransitionOnTime = TimeSpan.Zero;
            EnabledGestures = GestureType.Tap;

            _gameModel = gameModel;
            _gameModel.GameInitialized += new EventHandler(_gameModel_GameInitialized);
            _gameModel.EnemyAdded += new GameModel.EnemyAddedEventHandler(_gameModel_EnemyAdded);
            _gameModel.ClearingEnemies += new EventHandler(_gameModel_ClearingEnemies);
            //_gameModel.ScoreChanged += new EventHandler(_gameModel_ScoreChanged);
            //_gameModel.ScoreVisibilityChanged += new EventHandler(_gameModel_ScoreVisibilityChanged);
        }

        public override void Activate(bool instancePreserved)
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _font = _content.Load<SpriteFont>("MenuFont");
            _background = _content.Load<Texture2D>("Background");

            base.Activate(instancePreserved);
        }

        protected void _gameModel_GameInitialized(object sender, EventArgs e)
        {
            Components.Add(_gameModel.BugModel);
            Components.Add(_gameModel.FoodModel);
        }

        protected void _gameModel_EnemyAdded(object sender, EnemyAddedEventArgs e)
        {
            Components.Add(e.EnemyModel);
        }

        protected void _gameModel_ClearingEnemies(object sender, EventArgs e)
        {
            foreach (var enemy in _gameModel.EnemyList)
            {
                Components.Remove(enemy);
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_background, Vector2.Zero, Color.White);

            if (_gameModel.IsScoreVisible)
            {
                ScreenManager.SpriteBatch.DrawString(_font,
                                                     _gameModel.Score.ToString(),
                                                     Vector2.Zero,
                                                     Color.White);
            }

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Unload()
        {            
            //_gameModel.BugModel.ClearBehaviors();
            //_gameModel.FoodModel.Visible = false;
            //_gameModel.ClearEnemies();
            //_gameModel.IsScoreVisible = false;

            //ScreenManager.Game.Components.Remove(_gameModel.BugModel);

            if (_content != null)
                _content.Dispose();
            base.Unload();
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                if ((tl.State == TouchLocationState.Pressed)
                        || (tl.State == TouchLocationState.Moved))
                {
                    _gameModel.TargetPoint = tl.Position;
                }
            }
            base.HandleInput(gameTime, input);
        }

        private void _gameModel_GameOver(object sender, EventArgs e)
        {
            _gameModel.GameOver -= new EventHandler(_gameModel_GameOver);
            _gameModel.SendEnemyWave(10);
            _gameModel.BoostEnemySpeed();
            _gameModel.BugModel.ClearBehaviors();
            _gameModel.BugModel.Velocity = Vector2.Zero; //Make the bug stop right where it died
            var gameOverScreen = new GameOverScreen();
            gameOverScreen.Tapped += gameOverScreen_Tapped;
            ScreenManager.AddScreen(gameOverScreen, null);
        }

        private void gameOverScreen_Tapped(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new ReadyScreen(), null);
        }
    }
}
