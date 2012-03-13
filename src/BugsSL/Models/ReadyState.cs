using System.Windows;

namespace BugsSL.Models
{
    public class ReadyState : State
    {
        #region properties
        #endregion

        #region public methods
        public ReadyState(GameModel gameModel)
            : base(gameModel) { }

        public override void Enter()
        {
            _gameModel.BugModel.Position = new Point(400, 300);
            _gameModel.BugModel.Front = new Point(0, -1);            
        }

        public override void Update(float dt)
        {
            _gameModel.BugModel.Update(dt);
        }

        public override void Exit()
        {
            
        }
        #endregion

        #region private methods
        #endregion

        #region eventhandlers
        #endregion

        #region events
        #endregion

        #region private variables
        #endregion     
        

    }
}
