using Microsoft.Xna.Framework;

namespace BugsXNA.Models.States
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
