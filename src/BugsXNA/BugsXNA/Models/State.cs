using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BugsXNA.Models
{
    public abstract class State
    {
        public State(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public abstract void Enter();
        public abstract void Update(GameTime gameTime);
        public abstract void Exit();

        protected GameModel _gameModel;
    }
}
