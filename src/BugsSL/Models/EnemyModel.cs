namespace BugsSL.Models
{
    public class EnemyModel : AgentModel
    {
        /// <summary>
        /// Value between 0 and 1 that represents the enemies excitement level.
        /// This is meant to be used by a view to show excitement.
        /// </summary>
        public float Excitement { get; set; }
        
        public EnemyModel() : base() { }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
