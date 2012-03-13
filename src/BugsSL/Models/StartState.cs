using BugsSL.Behaviors;

namespace BugsSL.Models
{
    public class StartState : State
    {
        #region properties
        #endregion

        #region public methods
        public StartState(GameModel gameModel)
            : base(gameModel) { }

        public override void Enter()
        {
            _gameModel.BugModel.Add(new SeekPointBehavior(() => _gameModel.GetTarget(), _seekPointRampDistance));
        }

        public override void Update(float dt)
        {
            _gameModel.TargetPoint = _gameModel.FoodModel.Position;
            _gameModel.BugModel.Update(dt);

            if (Mathematics.Distance(_gameModel.BugModel.Position, _gameModel.FoodModel.Position) < _foodThreshold)
            {
                _gameModel.SetFood();
            }
        }

        public override void Exit()
        {
            _gameModel.BugModel.ClearBehaviors();
        }
        #endregion

        #region private methods
        #endregion

        #region eventhandlers
        #endregion

        #region events
        #endregion

        #region private variables
        private float _seekPointRampDistance = 50; //distance at which the bug will start slowing its approach.
        private float _foodThreshold = 20; //disttance at which the bug is considered to have eaten the food.
        #endregion     
        
    }
}
