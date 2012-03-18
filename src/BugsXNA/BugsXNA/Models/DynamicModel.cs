using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace BugsXNA.Models
{
    public class DynamicModel : DrawableGameComponent
    {
        public DynamicModel(Game game)
            : base(game)
        {
            //default
            Rotation = 0f;
            Position = Vector2.Zero;
            Scale = 1f;
            Front = Vector2.Zero;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            if (Texture != null)
                Texture.Dispose();
            Texture = null;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Rotation = (float)(Math.Atan2(Front.Y, Front.X) - Math.PI / 2.0);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2); //This to make the rotation point the center
            base.Draw(gameTime);
            BugsGame.Instance.SpriteBatch.Begin();
            BugsGame.Instance.SpriteBatch.Draw(Texture,
                                 Position,
                                 SourceRectangle,
                                 Color.White,
                                 Rotation,
                                 origin,
                                 Scale,
                                 SpriteEffects.None,
                                 1.0f);
            BugsGame.Instance.SpriteBatch.End();
        }

        public float Rotation { get; set; }
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public Vector2 Front { get; set; }
        protected Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; protected set; }
    }
}
