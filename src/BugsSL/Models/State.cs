namespace BugsSL.Models
{
    public abstract class State
    {
        public State(GameModel gameModel)
        {
          _gameModel = gameModel;
        }

        public abstract void Enter();
        public abstract void Update(float dt);
        public abstract void Exit();

        protected GameModel _gameModel;
    }
}
