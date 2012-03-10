using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugsXNA.Common;
using BugsXNA.Models;
using Microsoft.Xna.Framework;

namespace BugsXNA.Behaviors
{
    public class SeekPointBehavior : IBehavior
    {
        public AgentModel Agent { get; set; }
        public Vector2 SteeringForce { get; set; }

        public SeekPointBehavior(Func<Vector2> getPosition, float r)
        {
            rampDistance = r;
            this.getPosition = getPosition;
        }

        public void Update(GameTime gameTime)
        {
            SteeringForce = new Vector2(0, 0);
            position = getPosition();
            if (position.X == 0 && position.Y == 0) { return; }

            vectorDifference = Mathematics.Subtract(position, Agent.Position);
            distance = Mathematics.Length(vectorDifference);

            if (distance < 2)
            {
                return;
            }

            rampSpeed = Agent.MaxSpeed * (distance / rampDistance);
            rampSpeed = Math.Min(rampSpeed, Agent.MaxSpeed);

            vectorDifference = Mathematics.Multiply(vectorDifference, rampSpeed / distance);

            SteeringForce = Mathematics.Subtract(vectorDifference, Agent.Velocity);
        }

        private Vector2 vectorDifference;
        private float distance, rampSpeed, rampDistance;
        private Vector2 position;
        private Func<Vector2> getPosition;
    }
}
