using System;
using System.Windows.Input;
using System.Windows.Media;
using BugsSL.Models;
using BugsSL.Views;

namespace BugsSL.Controllers
{
    public class Controller
    {
        #region properties
        #endregion

        #region public methods
        public Controller(MainPage page)
        {
            _page = page;
        }

        public void Initialize()
        {
            _page.MouseMove += new MouseEventHandler(_page_MouseMove);
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            ShowGameScreen();
            ShowStartScreen();
        }
        #endregion

        #region private methods
        private void ShowGameScreen()
        {
            _gameModel = new GameModel();
            _gameView = new GameView(_gameModel);
            _gameModel.Width = _gameView.Width;
            _gameModel.Height = _gameView.Height;
            _gameModel.Initialize();
            _gameModel.Start();
            _page.LayoutRoot.Children.Add(_gameView);
        }

        private void ShowStartScreen()
        {
            _startView = new StartView();
            _startView.Clicked += new EventHandler(_startView_Clicked);
            _page.LayoutRoot.Children.Add(_startView);
            _gameModel.SetState(new StartState(_gameModel));
        }

        private void ShowReadyScreen()
        {
            _readyView = new ReadyView();
            _readyView.Clicked += new EventHandler(_readyView_Clicked);
            _page.LayoutRoot.Children.Add(_readyView);
            _gameModel.SetState(new ReadyState(_gameModel));
        }

        private void ShowPlayScreen()
        {
            _gameModel.GameOver += new EventHandler(_gameModel_GameOver);
            _gameModel.SetState(new PlayState(_gameModel));           
        }

        private void ShowGameOverScreen()
        {
            _gameOverView = new GameOverView();
            _gameOverView.Clicked += new EventHandler(_gameOverView_Clicked);
            _page.LayoutRoot.Children.Add(_gameOverView);
            _gameModel.SetState(new GameOverState(_gameModel));
        }

        public void Update(float dt)
        { 
            if (_gameModel != null) _gameModel.Update(dt);
            if (_gameView != null) _gameView.Update();
        }
        #endregion

        #region eventhandlers
        void _startView_Clicked(object sender, EventArgs e)
        {            
            _page.LayoutRoot.Children.Remove(_startView);
            _startView = null;
            ShowReadyScreen();
        }

        void _readyView_Clicked(object sender, EventArgs e)
        {
            _page.LayoutRoot.Children.Remove(_readyView);
            _readyView = null;
            ShowPlayScreen();
        }

        void _gameModel_GameOver(object sender, EventArgs e)
        {
            _gameModel.GameOver -= new EventHandler(_gameModel_GameOver);
             ShowGameOverScreen();
        }

        void _gameOverView_Clicked(object sender, EventArgs e)
        {
            _page.LayoutRoot.Children.Remove(_gameOverView);
            _gameOverView = null;
            ShowReadyScreen();            
        }

        void _page_MouseMove(object sender, MouseEventArgs e)
        {
            _gameModel.TargetPoint = e.GetPosition(_gameView);
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            _currentTime = DateTime.Now;
            _elapsedTime = _currentTime - _previousTime;
            _previousTime = _currentTime;
            Update(_elapsedTime.Milliseconds * .0003f);
        }
        #endregion

        #region events
        #endregion

        #region private variables
        MainPage _page;
        StartView _startView;
        ReadyView _readyView;
        GameOverView _gameOverView;

        GameView _gameView;
        GameModel _gameModel;

        DateTime _currentTime;
        DateTime _previousTime;
        TimeSpan _elapsedTime;
        #endregion                   
    }
}
