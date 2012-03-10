using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugsXNA.Common;
using BugsXNA.Models;
using Microsoft.Xna.Framework;

namespace BugsXNA.Behaviors
{
    public class SeperationBehavior : IBehavior
    {
        public AgentModel Agent { get; set; }
        public Vector2 SteeringForce { get; set; }

        public SeperationBehavior(float seperationDistance, List<AgentModel> agentList)
        {
            this.seperationDistance = seperationDistance;
            this.agentList = agentList;
        }

        public void Update(GameTime gameTime)
        {
            SteeringForce = new Vector2(0, 0);
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
        float seperationDistance, distance;
        Vector2 vectorDifference;
    }
}
