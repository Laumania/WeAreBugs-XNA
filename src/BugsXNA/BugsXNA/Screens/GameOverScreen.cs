using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugsXNA.Common.GameStateManagement;
using BugsXNA.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BugsXNA.Screens
{
    public class GameOverScreen : BaseScreen
    {
        public event EventHandler Tapped;

        private SpriteFont _font;
        private ContentManager _content;

        public GameOverScreen()
        {
            TransitionOffTime = TimeSpan.Zero;
            TransitionOnTime = TimeSpan.Zero;
            EnabledGestures = GestureType.Tap;
            IsPopup = true;
        }

        public override void Activate(bool instancePreserved)
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _font = _content.Load<SpriteFont>("MenuFont");

            base.Activate(instancePreserved);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(_font, "Game Over dude!", Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.End();
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    if (Tapped != null) Tapped(this, null);
                }
            }
            base.HandleInput(gameTime, input);
        }
    }
}
