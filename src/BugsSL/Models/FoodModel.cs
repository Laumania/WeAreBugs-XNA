namespace BugsSL.Models
{
    public class FoodModel : AgentModel
    {
        #region properties
        public bool IsVisible { get; set; }
        #endregion

        #region public methods
        public FoodModel() : base() 
        { 
        }

        public override void Initialize()
        {
            IsVisible = false;
            base.Initialize();
        }
        #endregion

        #region private methods
        #endregion

        #region eventhandlers
        #endregion

        #region events
        #endregion

        #region private variables
        #endregion     
        
    }
}
