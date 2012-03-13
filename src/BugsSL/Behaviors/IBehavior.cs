using System.Windows;
using BugsSL.Models;

namespace BugsSL.Behaviors
{
    public interface IBehavior
    {
        AgentModel Agent { get; set; }
        Point SteeringForce { get; set; }
        void Update();
    }
}
