using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugsXNA.Behaviors;
using BugsXNA.Common;
using Microsoft.Xna.Framework;

namespace BugsXNA.Models
{
    public class GameModel
    {
        #region properties
        public float Width { get; set; }
        public float Height { get; set; }
        public Vector2 TargetPoint { get; set; }
        public BugModel BugModel { get; set; }
        public FoodModel FoodModel { get; set; }
        public bool IsGameOver { get; set; }

        public bool IsScoreVisible
        {
            get { return _isScoreVisible; }
            set
            {
                _isScoreVisible = value;
                //if (ScoreVisibilityChanged != null) ScoreVisibilityChanged(this, null);
            }
        }

        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                //if (ScoreChanged != null) ScoreChanged(this, null);
            }
        }
        #endregion

        #region public methods
        public GameModel(Game game)
        {
            _game = game;
            //IsGameOver = false;
            //IsScoreVisible = false;
        }

        public void Initialize()
        {
            //init stuff
            InitializeBug(new Vector2(400, 320));
            InitializeFood();
            InitializeEnemies();
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
            float x = _foodPlacementBorderWidth + ((float)_rnd.NextDouble()) * (Width - 2 * _foodPlacementBorderWidth);
            float y = _foodPlacementBorderWidth + ((float)_rnd.NextDouble()) * (Height - 2 * _foodPlacementBorderWidth);
            FoodModel.Position = new Vector2(x, y);
        }

        public Vector2 GetTarget()
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
                _game.Components.Add(enemyModel);
                //if (EnemyAdded != null) EnemyAdded(this, new EnemyAddedEventArgs(enemyModel));
            }
        }

        public void ClearEnemies()
        {
            foreach (var enemy in _enemyList)
            {
                _game.Components.Remove(enemy);
            }
            _enemyList.Clear();
        }

        public void Update(GameTime gameTime)
        {
            if (_state != null) _state.Update(gameTime);
        }

        public void UpdateEnemies(GameTime gameTime)
        {
            bool bugIsCaught = false;
            foreach (EnemyModel enemyModel in _enemyList)
            {
                float distance = Mathematics.Distance(enemyModel.Position, BugModel.Position);
                enemyModel.Excitement = Math.Max(0, (_enemyExcitementThreshold - distance) / _enemyExcitementThreshold);
                //enemyModel.Update(gameTime);
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

        public void CheckForEatenFood(GameTime gameTime)
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
        private void InitializeBug(Vector2 startPoint)
        {
            BugModel = new BugModel(_game);
            BugModel.Front = new Vector2(0, -1);
            BugModel.Mass = .2f;
            BugModel.MaxSpeed = 4.5f;
            BugModel.MaxForce = .07f;
            BugModel.Position = startPoint;
            BugModel.Initialize();
            _game.Components.Add(BugModel);
        }

        private void InitializeFood()
        {
            FoodModel = new FoodModel(_game);
            FoodModel.Initialize();
            _game.Components.Add(FoodModel);
        }

        private void InitializeEnemies()
        {
            _enemyList = new List<AgentModel>();
        }

        private EnemyModel CreateEnemyModel()
        {
            EnemyModel enemyModel = new EnemyModel(_game);
            enemyModel.Front = new Vector2(0, -1);
            enemyModel.Mass = .2f;
            enemyModel.Scale = Mathematics.Lerp(.4f, .8f, (float)_rnd.NextDouble());

            enemyModel.MaxSpeed = (float)Mathematics.Lerp(_enemyMaxSpeedLowRange, _enemyMaxSpeedHighRange, (float)_rnd.NextDouble());
            enemyModel.MaxForce = (float)Mathematics.Lerp(_enemyMaxForceLowRange, _enemyMaxForceHighRange, (float)_rnd.NextDouble());

            int quadrant = _rnd.Next(1, 5);
            switch (quadrant)
            {
                case 1:
                    enemyModel.Position = new Vector2((float)_rnd.NextDouble() * Width, -_enemyCreationBufferWidth);
                    break;
                case 2:
                    enemyModel.Position = new Vector2(Width + _enemyCreationBufferWidth, (float)_rnd.NextDouble() * Height);
                    break;
                case 3:
                    enemyModel.Position = new Vector2((float)_rnd.NextDouble() * Width, Width + _enemyCreationBufferWidth);
                    break;
                default:
                    enemyModel.Position = new Vector2(-_enemyCreationBufferWidth, (float)_rnd.NextDouble() * Height);
                    break;
            }
            enemyModel.Initialize();
            return enemyModel;
        }

        #endregion

        #region eventhandlers
        #endregion

        #region events
        //public event EventHandler GameInitialized;
        public event EventHandler GameOver;
        //public event EventHandler ScoreChanged;
        //public event EventHandler ScoreVisibilityChanged;
        //public event EventHandler ClearingEnemies;
        //public event EnemyAddedEventHandler EnemyAdded;

        //public delegate void EnemyAddedEventHandler(object sender, EnemyAddedEventArgs e);
        #endregion

        #region private variables
        private Game _game;
        private List<AgentModel> _enemyList;
        private State _state;
        private float _foodThreshold = 20; //disttance at which the bug is considered to have eaten the food.
        private float _foodPlacementBorderWidth = 100f; //defines box where food can be placed.
        private float _enemyCreationBufferWidth = 20;
        private float _enemyAtBugThreshold = 20; //distance at which the enemy is considered to have caught the bug.
        private float _enemyPredictionFactor = 0f; //distance the enemy will "lead" the bug while pursuing.
        private float _enemySeperationFactor = 15f; //minimum distance between enemies.
        private float _enemyMaxSpeedLowRange = 1.5f; //low end value for random MaxSpeed
        private float _enemyMaxSpeedHighRange = 5.7f; //high end value for random MaxSpeed
        private float _enemyMaxForceLowRange = .01f; //low end value for random MaxForce
        private float _enemyMaxForceHighRange = .032f; //high end value for random MaxForce
        private float _enemyExcitementThreshold = 150; //the distance from the bug at which the enemy becomes excited (turns color)
        private Random _rnd = new Random();
        private bool _isScoreVisible;
        private int _score = 0;

        #endregion

    }
}
