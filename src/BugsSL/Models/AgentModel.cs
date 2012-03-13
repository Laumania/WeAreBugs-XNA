using System;
using System.Windows;
using System.Collections.Generic;
using BugsSL.Behaviors;

namespace BugsSL.Models
{
    public class AgentModel : DynamicModel
    {
        #region properties
        public Object Tag { get; set; }
        public Point Velocity { get; set; }
        public Point SteeringForce { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxForce { get; set; }
        public float VelocityAlignmentFactor { get; set; }
        #endregion

        #region public methods
        public AgentModel()            
        {
            //defaults
            Mass = .2f;
            MaxForce = .04f;
            MaxSpeed = 5;
            Scale = 1;
            VelocityAlignmentFactor = .3f;
        }

        public override void Initialize()
        {
            base.Initialize();
            behaviorList = new List<IBehavior>();
        }

        public void Add(IBehavior behavior)
        {
            behavior.Agent = this;
            behaviorList.Add(behavior);
        }

        public void Update(float dt)
        {
            SteeringForce = new Point(0, 0);
            foreach (var behavior in behaviorList)
            {
                behavior.Update();
                SteeringForce = Mathematics.Add(SteeringForce, behavior.SteeringForce);
            }

            SteeringForce = Mathematics.Truncate(SteeringForce, MaxForce);

            if (Mathematics.Length(SteeringForce) > _steeringForceThreshold)
            {
                Point acceleration = Mathematics.Multiply(SteeringForce, 1f / Mass);

                Velocity = Mathematics.Add(Velocity, acceleration);

                Position = Mathematics.Add(Position, Velocity);

                float speed = Mathematics.Length(Velocity);
                if (speed > _speedThreshold)
                {
                    Point velocity = Mathematics.Normalize(Velocity);
                    velocity = Mathematics.Lerp(Front, velocity, VelocityAlignmentFactor);
                    Front = Mathematics.Normalize(velocity);
                }
            }
            else
            {
                Velocity = new Point(0, 0);
            }
        }

        public void ClearBehaviors()
        {
            behaviorList.Clear();
        }
        #endregion

        #region private methods
        #endregion

        #region eventhandlers
        #endregion

        #region events
        
        #endregion

        #region private variables
        private List<IBehavior> behaviorList;
        private float _steeringForceThreshold = .001f;
        private float _speedThreshold = .2f;
        #endregion 
    }
}
