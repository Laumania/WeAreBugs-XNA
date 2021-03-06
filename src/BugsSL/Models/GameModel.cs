﻿using System;
using System.Windows;
using System.Collections.Generic;
using BugsSL.Behaviors;

namespace BugsSL.Models
{
    public class GameModel
    {
        #region properties
        public double Width { get; set; }
        public double Height { get; set; }
        public Point TargetPoint { get; set; }
        public BugModel BugModel { get; set; }
        public FoodModel FoodModel { get; set; }
        public bool IsGameOver { get; set; }

        public bool IsScoreVisible 
        {
            get { return _isScoreVisible; }
            set
            {
                _isScoreVisible = value;
                if (ScoreVisibilityChanged!=null) ScoreVisibilityChanged(this, null);
            }
        }

        public int Score
        {
            get { return _score; }
            set 
            {
                _score = value;
                if (ScoreChanged != null) ScoreChanged(this, null);
            }
        }
        #endregion

        #region public methods
        public GameModel()
        {
            IsGameOver = false;
            IsScoreVisible = false;
        }

        public void Initialize()
        { 
            //init stuff
            InitializeBug(new Point(400, 320));
            InitializeFood();
            InitializeEnemies();
            GameInitialized(this, null);
        }

        public void Start()
        {
            SetFood();   
        }

        public void SetState(State state)
        {
            if (_state != null)
            {
                _state.Exit();
            }
                _state = state;
                _state.Enter();
        }

        public void SetFood()
        {
            double x = _foodPlacementBorderWidth + _rnd.NextDouble() * (Width - 2*_foodPlacementBorderWidth);
            double y = _foodPlacementBorderWidth + _rnd.NextDouble() * (Height -2*_foodPlacementBorderWidth);
            FoodModel.Position = new Point(x, y); 
        }

        public Point GetTarget()
        {
            return TargetPoint;
        }

        public void SendEnemyWave(int n)
        {
            for (int i = 0; i < n; i++)
            {
                EnemyModel enemyModel = CreateEnemyModel();
                enemyModel.Add(new PursueAgentBehavior(_enemyAtBugThreshold, _enemyPredictionFactor, BugModel));
                enemyModel.Add(new SeperationBehavior(_enemySeperationFactor, _enemyList));
                _enemyList.Add(enemyModel);
                if (EnemyAdded != null) EnemyAdded(this, new EnemyAddedEventArgs(enemyModel));
            }
        }

        public void ClearEnemies()
        {
            if (ClearingEnemies != null) ClearingEnemies(this, null);
            _enemyList.Clear();
        }

        public void Update(float dt)
        {
            if (_state != null) _state.Update(dt);
        }

        public void UpdateEnemies(float dt)
        {
            bool bugIsCaught = false;
            foreach (EnemyModel enemyModel in _enemyList)
            {
                float distance = Mathematics.Distance(enemyModel.Position, BugModel.Position);
                enemyModel.Excitement = Math.Max(0, (_enemyExcitementThreshold - distance) / _enemyExcitementThreshold);
                enemyModel.Update(dt);
                if (distance < 15)
                {
                    bugIsCaught = true;
                }
            }
            if (!IsGameOver)
            {
                if (bugIsCaught && GameOver != null)
                {
                    IsGameOver = true;
                    GameOver(this, null);
                }
            }            
        }

        public void CheckForEatenFood(float dt)
        {
            if (Mathematics.Distance(BugModel.Position, FoodModel.Position) < _foodThreshold)
            {
                SetFood();
                SendEnemyWave(1);
                Score += 1;
            }
        }

        public void BoostEnemySpeed()
        {
            foreach (EnemyModel enemyModel in _enemyList)
            {
                enemyModel.MaxForce = .035f;
                enemyModel.MaxSpeed = 6;
            }
        }
        #endregion

        #region private methods
        private void InitializeBug(Point startPoint)
        {
            BugModel = new BugModel();
            BugModel.Front = new Point(0, -1);
            BugModel.Mass = .2f;
            BugModel.MaxSpeed = 4.5f;
            BugModel.MaxForce = .07f;
            BugModel.Position = startPoint;
            BugModel.Initialize();
        }

        private void InitializeFood()
        {
            FoodModel = new FoodModel();
            FoodModel.Initialize();
        }

        private void InitializeEnemies()
        {
            _enemyList = new List<AgentModel>();
        }

        private EnemyModel CreateEnemyModel()
        {
            EnemyModel enemyModel = new EnemyModel();
            enemyModel.Front = new Point(0, -1);
            enemyModel.Mass = .2f;
            enemyModel.Scale = Mathematics.Lerp(.4d, .8d, _rnd.NextDouble());            

            enemyModel.MaxSpeed = (float)Mathematics.Lerp(_enemyMaxSpeedLowRange, _enemyMaxSpeedHighRange, _rnd.NextDouble());
            enemyModel.MaxForce = (float)Mathematics.Lerp(_enemyMaxForceLowRange, _enemyMaxForceHighRange, _rnd.NextDouble());

            int quadrant = _rnd.Next(1, 5);
            switch (quadrant)
            {
                case 1:
                    enemyModel.Position = new Point(_rnd.NextDouble() * Width, -_enemyCreationBufferWidth);
                    break;
                case 2:
                    enemyModel.Position = new Point(Width + _enemyCreationBufferWidth, _rnd.NextDouble() * Height);
                    break;
                case 3:
                    enemyModel.Position = new Point(_rnd.NextDouble() * Width, Width + _enemyCreationBufferWidth);
                    break;
                default:
                    enemyModel.Position = new Point(-_enemyCreationBufferWidth, _rnd.NextDouble() * Height);
                    break;
            }
            enemyModel.Initialize();
            return enemyModel;
        }

        #endregion

        #region eventhandlers
        #endregion

        #region events
        public event EventHandler GameInitialized;
        public event EventHandler GameOver;
        public event EventHandler ScoreChanged;
        public event EventHandler ScoreVisibilityChanged;
        public event EventHandler ClearingEnemies;
        public event EnemyAddedEventHandler EnemyAdded;

        public delegate void EnemyAddedEventHandler(object sender, EnemyAddedEventArgs e);
        #endregion

        #region private variables       
        private List<AgentModel> _enemyList;
        private State _state;
        private float _foodThreshold = 20; //disttance at which the bug is considered to have eaten the food.
        private double _foodPlacementBorderWidth = 100; //defines box where food can be placed.
        private double _enemyCreationBufferWidth = 20;
        private float _enemyAtBugThreshold = 20; //distance at which the enemy is considered to have caught the bug.
        private float _enemyPredictionFactor = 0f; //distance the enemy will "lead" the bug while pursuing.
        private float _enemySeperationFactor = 15f; //minimum distance between enemies.
        private double _enemyMaxSpeedLowRange = 1.5d; //low end value for random MaxSpeed
        private double _enemyMaxSpeedHighRange = 5.7d; //high end value for random MaxSpeed
        private double _enemyMaxForceLowRange = .01d; //low end value for random MaxForce
        private double _enemyMaxForceHighRange = .032d; //high end value for random MaxForce
        private float _enemyExcitementThreshold = 150; //the distance from the bug at which the enemy becomes excited (turns color)
        private Random _rnd = new Random();
        private bool _isScoreVisible;
        private int _score = 0;
        #endregion     
        
    }

    public class EnemyAddedEventArgs : EventArgs
    {
        public EnemyModel EnemyModel { get; private set; }
        public EnemyAddedEventArgs(EnemyModel enemyModel)
        {
            this.EnemyModel = enemyModel;
        }
    }
}
