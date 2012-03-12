using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugsXNA.Behaviors;
using Microsoft.Xna.Framework;

namespace BugsXNA.Models
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
            _gameModel.FoodModel.Visible = true;
            _gameModel.IsGameOver = false;
            _gameModel.IsScoreVisible = true;
            _gameModel.Score = 0;
        }

        public override void Update(GameTime gameTime)
        {
            //_gameModel.BugModel.Update(gameTime);
            _gameModel.CheckForEatenFood(gameTime);
            _gameModel.UpdateEnemies(gameTime);
        }

        public override void Exit()
        {
            _gameModel.BugModel.ClearBehaviors();
            _gameModel.FoodModel.Visible = false;
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
