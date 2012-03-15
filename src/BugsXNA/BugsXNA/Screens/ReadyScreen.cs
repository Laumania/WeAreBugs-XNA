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

        //private Texture2D _background;
        //private GameModel _gameModel;
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
            //_background = _content.Load<Texture2D>("Background");

            //_gameModel = new GameModel(ScreenManager.Game);
            //_gameModel.Width = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            //_gameModel.Height = ScreenManager.Game.GraphicsDevice.Viewport.Height;
            //_gameModel.Initialize();
            //_gameModel.Start();

            //_gameModel.BugModel.Position = new Vector2(400, 300);
            //_gameModel.BugModel.Front = new Vector2(0, -1);

            base.Activate(instancePreserved);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();
            //ScreenManager.SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.DrawString(_font, "Eat the white food dots for points", Vector2.Zero, Color.White);
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
