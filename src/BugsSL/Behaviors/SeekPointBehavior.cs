using System;
using System.Windows;
using BugsSL.Models;

namespace BugsSL.Behaviors
{
    public class PursueAgentBehavior : IBehavior
    {
        public AgentModel Agent { get; set; }
        public AgentModel TargetAgent;
        public Point SteeringForce { get; set; }

        public PursueAgentBehavior(float rampDistance, float predictionFactor, AgentModel targetAgent)
        {
            this.rampDistance = rampDistance;
            this.predictionFactor = predictionFactor;
            TargetAgent = targetAgent;
        }

        public void Update()
        {
            SteeringForce = new Point(0, 0);
            vectorDifference = Mathematics.Subtract(TargetAgent.Position, Agent.Position);
            distance = Mathematics.Length(vectorDifference);

            Point predictedPosition = Mathematics.Multiply(TargetAgent.Velocity, distance * predictionFactor);
            Point targetPosition = Mathematics.Add(TargetAgent.Position, predictedPosition);

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

        private Point vectorDifference, force;
        private float distance, rampSpeed, rampDistance, predictionFactor;
    }
}
