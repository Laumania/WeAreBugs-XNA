using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BugsXNA.Models;
using Microsoft.Xna.Framework;

namespace BugsXNA.Behaviors
{
    public interface IBehavior
    {
        AgentModel Agent { get; set; }
        Vector2 SteeringForce { get; set; }
        void Update(GameTime gameTime);
    }
}
