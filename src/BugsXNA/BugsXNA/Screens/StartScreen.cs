using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using BugsXNA.Behaviors;
using BugsXNA.Common;
using BugsXNA.Common.GameStateManagement;
using BugsXNA.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BugsXNA.Screens
{
    public class StartScreen : BaseScreen
    {
        public event EventHandler Tapped;

        private ContentManager _content;
        private SpriteFont _font;
        
        public StartScreen()
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

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(_font, 
                                                 "'To a good approximation, all species are insects' -Robert May", 
                                                 new Vector2(70, 100), 
                                                 Color.White);

            ScreenManager.SpriteBatch.DrawString(_font,
                                                 "\"We Are Bugs!\"",
                                                 new Vector2(300, 300),
                                                 Color.White);

            ScreenManager.SpriteBatch.DrawString(_font,
                                                 "Created by Jeff Weber",
                                                 new Vector2(270, 350),
                                                 Color.White);

            ScreenManager.SpriteBatch.DrawString(_font,
                                                 "Ported to XNA by Mads Laumann",
                                                 new Vector2(230, 380),
                                                 Color.White);
            ScreenManager.SpriteBatch.End();
        }

        public override void Unload()
        {
            //ScreenManager.Game.Components.Remove(_gameModel.BugModel);

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
