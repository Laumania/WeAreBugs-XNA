using System.Windows;
using System.Collections.Generic;
using BugsSL.Models;

namespace BugsSL.Behaviors
{
    public class SeperationBehavior : IBehavior
    {
        public AgentModel Agent { get; set; }
        public Point SteeringForce { get; set; }

        public SeperationBehavior(float seperationDistance, List<AgentModel> agentList)
        {
            this.seperationDistance = seperationDistance;
            this.agentList = agentList;
        }

        public void Update()
        {
            SteeringForce = new Point(0, 0);
            foreach (var agent in agentList)
            {
                vectorDifference = Mathematics.Subtract(Agent.Position, agent.Position);
                distance = Mathematics.Length(vectorDifference);
                if (distance < seperationDistance && distance > 0)
                {
                    vectorDifference = Mathematics.Normalize(vectorDifference);
                    vectorDifference = Mathematics.Multiply(vectorDifference, 50 / (float)(distance));
                    SteeringForce = Mathematics.Add(SteeringForce, vectorDifference);
                }
            }
        }
        List<AgentModel> agentList;
        double seperationDistance, distance;
        Point vectorDifference;
    }
}
