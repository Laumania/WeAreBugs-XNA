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
    public class PlayScreen : GameScreen
    {
        private Texture2D _background;
        private GameModel _gameModel;
        private ContentManager _content;
        private SpriteFont _font;
        private float _seekPointRampDistance = 50; //distance at which the bug will start slowing its approach.

        public PlayScreen()
        {
            TransitionOffTime = TimeSpan.Zero;
            TransitionOnTime = TimeSpan.Zero;
            EnabledGestures = GestureType.Tap;
        }

        public override void Activate(bool instancePreserved)
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _font = _content.Load<SpriteFont>("MenuFont");
            _background = _content.Load<Texture2D>("Background");

            _gameModel = new GameModel(ScreenManager.Game);
            _gameModel.Width = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            _gameModel.Height = ScreenManager.Game.GraphicsDevice.Viewport.Height;
            _gameModel.Initialize();
            _gameModel.Start();
            _gameModel.BugModel.Add(new SeekPointBehavior(() => _gameModel.GetTarget(), _seekPointRampDistance));
            _gameModel.FoodModel.Visible = true;
            _gameModel.IsGameOver = false;
            _gameModel.IsScoreVisible = true;
            _gameModel.Score = 0;
            _gameModel.GameOver += new EventHandler(_gameModel_GameOver);

            _gameModel.BugModel.Position = new Vector2(400, 300);
            _gameModel.BugModel.Front = new Vector2(0, -1);

            base.Activate(instancePreserved);
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _gameModel.CheckForEatenFood(gameTime);
            _gameModel.UpdateEnemies(gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.End();
        }

        public override void Unload()
        {            
            _gameModel.BugModel.ClearBehaviors();
            _gameModel.FoodModel.Visible = false;
            _gameModel.ClearEnemies();
            _gameModel.IsScoreVisible = false;

            ScreenManager.Game.Components.Remove(_gameModel.BugModel);

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
