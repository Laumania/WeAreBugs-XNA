using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugsXNA.Common;
using BugsXNA.Common.GameStateManagement;
using BugsXNA.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BugsXNA.Screens
{
    public class ReadyScreen : BaseScreen
    {
        public event EventHandler Tapped;

        private ContentManager _content;
        private SpriteFont _font;

        public ReadyScreen()
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
            ScreenManager.SpriteBatch.DrawString(_font, 
                                                 "Eat the white food dots for points", 
                                                 new Vector2(220, 200), 
                                                 Color.White);

            ScreenManager.SpriteBatch.DrawString(_font,
                                                 "Steer by touching the screen where you want to bug to go",
                                                 new Vector2(100, 350),
                                                 Color.White);

            ScreenManager.SpriteBatch.End();
        }

        public override void Unload()
        {
            if (_content != null)
                _content.Dispose();
            base.Unload();
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
