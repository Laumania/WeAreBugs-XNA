using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugsXNA.Common;
using BugsXNA.Common.GameStateManagement;
using Microsoft.Xna.Framework;

namespace BugsXNA.Screens
{
    public abstract class BaseScreen : GameScreen
    {
        protected BaseScreen()
        {
            Components = new GameComponentManager(BugsGame.Instance);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            Components.Update(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            Components.Draw(gameTime);
            base.Draw(gameTime);
        }

        public GameComponentManager Components { get; private set; }
    }
}
