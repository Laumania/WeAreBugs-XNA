using System;
using System.Windows;
using BugsSL.Models;

namespace BugsSL.Behaviors
{
    public class SeekPointBehavior : IBehavior
    {
        public AgentModel Agent { get; set; }
        public Point SteeringForce { get; set; }

        public SeekPointBehavior(Func<Point> getPosition, float r)
        {
            rampDistance = r;
            this.getPosition = getPosition;
        }

        public void Update()
        {
            SteeringForce = new Point(0, 0);
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

        private Point vectorDifference;
        private float distance, rampSpeed, rampDistance;
        private Point position;
        private Func<Point> getPosition;
    }
}
