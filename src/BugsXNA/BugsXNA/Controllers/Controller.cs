using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugsXNA.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace BugsXNA.Controllers
{
    public class Controller
    {
        #region properties
        #endregion

        #region public methods
        public Controller(Game game)
        {
            _game = game;
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
            _gameModel = new GameModel(_game);
            //_gameView = new GameView(_gameModel);
            _gameModel.Width = _game.GraphicsDevice.Viewport.Width;
            _gameModel.Height = _game.GraphicsDevice.Viewport.Height;
            _gameModel.Initialize();
            _gameModel.Start();
            //_page.LayoutRoot.Children.Add(_gameView);
        }

        private void ShowStartScreen()
        {
            //_startView = new StartView();
            //_startView.Clicked += new EventHandler(_startView_Clicked);
            //_page.LayoutRoot.Children.Add(_startView);
            var startState = new StartState(_gameModel);
            startState.Clicked += new EventHandler(_startView_Clicked);
            _gameModel.SetState(startState);
        }

        private void ShowReadyScreen()
        {
            //_readyView = new ReadyView();
            //_readyView.Clicked += new EventHandler(_readyView_Clicked);
            //_page.LayoutRoot.Children.Add(_readyView);
            var readyState = new ReadyState(_gameModel);
            readyState.Clicked += new EventHandler(_readyView_Clicked);
            _gameModel.SetState(readyState);
        }

        private void ShowPlayScreen()
        {
            _gameModel.GameOver += new EventHandler(_gameModel_GameOver);
            _gameModel.SetState(new PlayState(_gameModel));
        }

        private void ShowGameOverScreen()
        {
            //_gameOverView = new GameOverView();
            //_gameOverView.Clicked += new EventHandler(_gameOverView_Clicked);
            //_page.LayoutRoot.Children.Add(_gameOverView);
            var gameOverState = new GameOverState(_gameModel);
            gameOverState.Clicked += new EventHandler(_gameOverView_Clicked);
            _gameModel.SetState(gameOverState);
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
        void _startView_Clicked(object sender, EventArgs e)
        {
            //_page.LayoutRoot.Children.Remove(_startView);
            //_startView = null;
            ShowReadyScreen();
        }

        void _readyView_Clicked(object sender, EventArgs e)
        {
            //_page.LayoutRoot.Children.Remove(_readyView);
            //_readyView = null;
            ShowPlayScreen();
        }

        void _gameModel_GameOver(object sender, EventArgs e)
        {
            _gameModel.GameOver -= new EventHandler(_gameModel_GameOver);
            ShowGameOverScreen();
        }

        void _gameOverView_Clicked(object sender, EventArgs e)
        {
            //_page.LayoutRoot.Children.Remove(_gameOverView);
            //_gameOverView = null;
            ShowReadyScreen();
        }

        #endregion

        #region events
        #endregion

        #region private variables
        Game _game;
        //StartView _startView;
        //ReadyView _readyView;
        //GameOverView _gameOverView;

        //GameView _gameView;
        GameModel _gameModel;

        //DateTime _currentTime;
        //DateTime _previousTime;
        //TimeSpan _elapsedTime;
        #endregion
    }
}
