using BugsSL.Behaviors;

namespace BugsSL.Models
{
    public class PlayState : State 
    {
        #region properties
        #endregion

        #region public methods
        public PlayState(GameModel gameModel)
            : base(gameModel) { }

        public override void Enter()
        {
            _gameModel.BugModel.Add(new SeekPointBehavior(() => _gameModel.GetTarget(), _seekPointRampDistance));
            _gameModel.FoodModel.IsVisible = true;
            _gameModel.IsGameOver = false;
            _gameModel.IsScoreVisible = true;
            _gameModel.Score = 0;
        }

        public override void Update(float dt)
        {
            _gameModel.BugModel.Update(dt);
            _gameModel.CheckForEatenFood(dt);
            _gameModel.UpdateEnemies(dt);
        }

        public override void Exit()
        {
            _gameModel.BugModel.ClearBehaviors();
            _gameModel.FoodModel.IsVisible = false;
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
        #endregion     
        
    

}
}
