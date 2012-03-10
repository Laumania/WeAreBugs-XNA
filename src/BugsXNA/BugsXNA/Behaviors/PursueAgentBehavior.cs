using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugsXNA.Common;
using BugsXNA.Models;
using Microsoft.Xna.Framework;

namespace BugsXNA.Behaviors
{
    public class PursueAgentBehavior : IBehavior
    {
        public AgentModel Agent { get; set; }
        public AgentModel TargetAgent;
        public Vector2 SteeringForce { get; set; }

        public PursueAgentBehavior(float rampDistance, float predictionFactor, AgentModel targetAgent)
        {
            this.rampDistance = rampDistance;
            this.predictionFactor = predictionFactor;
            TargetAgent = targetAgent;
        }

        public void Update(GameTime gameTime)
        {
            SteeringForce = new Vector2(0, 0);
            vectorDifference = Mathematics.Subtract(TargetAgent.Position, Agent.Position);
            distance = Mathematics.Length(vectorDifference);

            Vector2 predictedPosition = Mathematics.Multiply(TargetAgent.Velocity, distance * predictionFactor);
            Vector2 targetPosition = Mathematics.Add(TargetAgent.Position, predictedPosition);

            vectorDifference = Mathematics.Subtract(targetPosition, Agent.Position);
            distance = Mathematics.Length(vectorDifference);

            if (distance < 2)
            {
                return;
            }

            rampSpeed = Agent.MaxSpeed * (distance / rampDistance);
            rampSpeed = Math.Min(rampSpeed, Agent.MaxSpeed);

            vectorDifference = Mathematics.Multiply(vectorDifference, rampSpeed / distance);

            force = Mathematics.Subtract(vectorDifference, Agent.Velocity);
            SteeringForce = force;
        }

        private Vector2 vectorDifference, force;
        private float distance, rampSpeed, rampDistance, predictionFactor;
    }
}
