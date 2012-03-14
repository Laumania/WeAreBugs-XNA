using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace BugsXNA.Models
{
    public class ReadyState : State
    {
        public event EventHandler Clicked;

        #region properties
        #endregion

        #region public methods
        public ReadyState(GameModel gameModel)
            : base(gameModel) { }

        public override void Enter()
        {
            _gameModel.BugModel.Position = new Vector2(400, 300);
            _gameModel.BugModel.Front = new Vector2(0, -1);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsScreenTapped())
            {
                if (Clicked != null) Clicked(this, null);
            }
            //_gameModel.BugModel.Update(gameTime);
        }

        public override void Exit()
        {

        }
        #endregion

        #region private methods
        private bool IsScreenTapped()
        {
            if(TouchPanel.IsGestureAvailable)
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