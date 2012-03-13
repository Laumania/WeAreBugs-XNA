using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BugsSL.Models;

namespace BugsSL.Views
{
    public partial class GameView : UserControl
    {
        #region properties
        #endregion

        #region public methods
        public GameView(GameModel gameModel)
        {
            InitializeComponent();
            _gameModel = gameModel;
            _gameModel.GameInitialized += new EventHandler(_gameModel_GameInitialized);
            _gameModel.EnemyAdded += new GameModel.EnemyAddedEventHandler(_gameModel_EnemyAdded);
            _gameModel.ClearingEnemies += new EventHandler(_gameModel_ClearingEnemies);
            _gameModel.ScoreChanged += new EventHandler(_gameModel_ScoreChanged);
            _gameModel.ScoreVisibilityChanged += new EventHandler(_gameModel_ScoreVisibilityChanged);
        }

        public void Initialize()
        {
            _bugView = new BugView(_gameModel.BugModel);
            _foodView = new FoodView(_gameModel.FoodModel);
            _enemyViewList = new List<EnemyView>();
            
            LayoutRoot.Children.Add(_bugView);
            LayoutRoot.Children.Add(_foodView);
        }

        public void ClearEnemyViewList()
        {
            foreach (var enemyView in _enemyViewList)
            {
                LayoutRoot.Children.Remove(enemyView);
            }
            _enemyViewList.Clear();
        }

        public void Update()
        {
            _bugView.Update();
            _foodView.Update();
            foreach (var enemyView in _enemyViewList)
            {
                enemyView.Update();
            }
        }
        #endregion

        #region private methods
        private void CreateEnemyView(EnemyModel enemyModel)
        {
            EnemyView enemyView = new EnemyView(enemyModel);
            LayoutRoot.Children.Add(enemyView);
            _enemyViewList.Add(enemyView);
        }
        #endregion

        #region eventhandlers
        void _gameModel_GameInitialized(object sender, EventArgs e)
        {
            Initialize();
        }

        void _gameModel_EnemyAdded(object sender, EnemyAddedEventArgs e)
        {
            CreateEnemyView(e.EnemyModel);
        }

        void _gameModel_ClearingEnemies(object sender, EventArgs e)
        {
            ClearEnemyViewList();
        }

        void _gameModel_ScoreVisibilityChanged(object sender, EventArgs e)
        {
            if (_gameModel.IsScoreVisible)
            {
                ScoreTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                ScoreTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        void _gameModel_ScoreChanged(object sender, EventArgs e)
        {
            ScoreTextBlock.Text = _gameModel.Score.ToString("00");
        }
        #endregion

        #region events
        #endregion

        #region private variables
        GameModel _gameModel;

        BugView _bugView;
        FoodView _foodView;
        List<EnemyView> _enemyViewList;
        #endregion
    }
}
