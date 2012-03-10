using System;
using System.Collections.Generic;
using System.Linq;
using BugsXNA.Behaviors;
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
    public class AgentModel : DynamicModel
    {
        private List<IBehavior> behaviorList;
        private float _steeringForceThreshold = .001f;
        private float _speedThreshold = .2f;

        public AgentModel(Game game)
            : base(game)
        {
            //defaults
            Mass = .2f;
            MaxForce = .04f;
            MaxSpeed = 5;
            Scale = 1;
            VelocityAlignmentFactor = .3f;

            behaviorList = new List<IBehavior>();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            SteeringForce = new Vector2(0, 0);
            foreach (var behavior in behaviorList)
            {
                behavior.Update(gameTime);
                SteeringForce = Mathematics.Add(SteeringForce, behavior.SteeringForce);
            }

            SteeringForce = Mathematics.Truncate(SteeringForce, MaxForce);

            if (Mathematics.Length(SteeringForce) > _steeringForceThreshold)
            {
                Vector2 acceleration = Mathematics.Multiply(SteeringForce, 1f / Mass);
                Velocity = Mathematics.Add(Velocity, acceleration);
                Position = Mathematics.Add(Position, Velocity);
                Vector2.Multiply(Position, (float)gameTime.ElapsedGameTime.TotalSeconds);

                float speed = Mathematics.Length(Velocity);
                if (speed > _speedThreshold)
                {
                    Vector2 velocity = Mathematics.Normalize(Velocity);
                    velocity = Mathematics.Lerp(Front, velocity, VelocityAlignmentFactor);
                    Front = Mathematics.Normalize(velocity);
                }
            }
            else
            {
                Velocity = new Vector2(0, 0);
            }
        }

        public void Add(IBehavior behavior)
        {
            behavior.Agent = this;
            behaviorList.Add(behavior);
        }

        public void ClearBehaviors()
        {
            behaviorList.Clear();
        }

        public Object Tag { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 SteeringForce { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxForce { get; set; }
        public float VelocityAlignmentFactor { get; set; }
    }
}
