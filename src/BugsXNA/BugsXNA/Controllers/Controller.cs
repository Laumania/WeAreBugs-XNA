using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugsXNA.Models;
using BugsXNA.Models.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using BugsXNA.Screens;
using BugsXNA.Common.GameStateManagement;

namespace BugsXNA.Controllers
{
    public class Controller
    {
        #region properties
        #endregion

        #region public methods
        public Controller(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }

        public void Initialize()
        {
            ShowGameScreen();
            ShowStartScreen();
        }
        #endregion

        #region private methods
        private void ShowGameScreen()
        {
            _gameModel = new GameModel(_screenManager.Game);
            _gameModel.Width = _screenManager.Game.GraphicsDevice.Viewport.Width;
            _gameModel.Height = _screenManager.Game.GraphicsDevice.Viewport.Height;
            _gameModel.Initialize();
            _gameModel.Start();
            _screenManager.AddScreen(new PlayScreen(_gameModel), null);
            //_page.LayoutRoot.Children.Add(_gameView);
        }

        private void ShowStartScreen()
        {
            //_startView = new StartView();
            //_startView.Tapped += new EventHandler(_startScreen_Tapped);
            //_page.LayoutRoot.Children.Add(_startView);

            var startScreen = new StartScreen();
            startScreen.Tapped += new EventHandler(_startScreen_Tapped);
            _screenManager.AddScreen(startScreen, null);

            var startState = new StartState(_gameModel);
            _gameModel.SetState(startState);
        }

        private void ShowReadyScreen()
        {
            var readyScreen = new ReadyScreen();
            readyScreen.Tapped += new EventHandler(_readyScreen_Tapped);
            _screenManager.AddScreen(readyScreen, null);

            _gameModel.SetState(new ReadyState(_gameModel));
        }

        private void ShowPlayScreen()
        {
            _gameModel.GameOver += new EventHandler(_gameModel_GameOver);
            _gameModel.SetState(new PlayState(_gameModel));
        }

        private void ShowGameOverScreen()
        {
            var gameOverScreen = new GameOverScreen();
            gameOverScreen.Tapped += new EventHandler(_gameOverView_Clicked);
            _screenManager.AddScreen(gameOverScreen, null);

            _gameModel.SetState(new GameOverState(_gameModel));
        }

        public void Update(GameTime gameTime)
        {
            UpdateTargetPoint();
            if (_gameModel != null) _gameModel.Update(gameTime);
            //if (_gameView != null) _gameView.Update();
        }

        private void UpdateTargetPoint()
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
        }

        #endregion

        #region eventhandlers
        void _startScreen_Tapped(object sender, EventArgs e)
        {
            (sender as GameScreen).ExitScreen();
            ShowReadyScreen();
        }

        void _readyScreen_Tapped(object sender, EventArgs e)
        {
            (sender as GameScreen).ExitScreen();
            ShowPlayScreen();
        }

        void _gameModel_GameOver(object sender, EventArgs e)
        {
            _gameModel.GameOver -= new EventHandler(_gameModel_GameOver);
            ShowGameOverScreen();
        }

        void _gameOverView_Clicked(object sender, EventArgs e)
        {
            (sender as GameScreen).ExitScreen();
            ShowReadyScreen();
        }

        #endregion

        #region events
        #endregion

        #region private variables

        private ScreenManager _screenManager;
        private GameModel _gameModel;
        //StartView _startView;
        //ReadyView _readyView;
        //GameOverView _gameOverView;
        //GameView _gameView;
        #endregion
    }
}
