using Microsoft.Xna.Framework;

namespace BugsXNA.Models.States
{
    public class GameOverState : State
    {
        #region properties
        public GameOverState(GameModel gameModel)
            : base(gameModel) { }

        #endregion

        #region public methods
        public override void Enter()
        {
            _gameModel.SendEnemyWave(10);
            _gameModel.BoostEnemySpeed();
        }

        public override void Update(GameTime gameTime)
        {
            _gameModel.UpdateEnemies(gameTime);
        }

        public override void Exit()
        {
            _gameModel.ClearEnemies();
            _gameModel.IsScoreVisible = false;
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
