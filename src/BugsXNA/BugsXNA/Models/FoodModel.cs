using System;
using System.Collections.Generic;
using System.Linq;
using BugsXNA.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace BugsXNA.Models
{
    public class FoodModel : AgentModel
    {
        public FoodModel(Game game)
            : base(game)
        {
            
        }
       
        public override void Initialize()
        {
            this.Visible = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            var spriteSheet = Game.Content.Load<SpriteSheet>("SpriteSheet1");
            Texture = spriteSheet.Texture;
            SourceRectangle = spriteSheet.SourceRectangle("Food");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
