using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace BugsXNA.Models
{
    public class GameOverState : State
    {
        public event EventHandler Clicked;

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
            if (IsScreenTapped())
            {
                if (Clicked != null) Clicked(this, null);
            }
            _gameModel.UpdateEnemies(gameTime);
        }

        public override void Exit()
        {
            _gameModel.ClearEnemies();
            _gameModel.IsScoreVisible = false;
        }
        #endregion

        #region private methods
        private bool IsScreenTapped()
        {
            if (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();
                if (gesture.GestureType == GestureType.Tap)
                    return true;
            }

            return false;
        }
        #endregion

        #region eventhandlers
        #endregion

        #region events
        #endregion

        #region private variables
        #endregion
    }
}
