using System.Windows;
using System.Windows.Controls;
using BugsSL.Models;

namespace BugsSL.Views
{
    public partial class FoodView : UserControl
    {
        #region properties
        public bool IsVisible
        {
            get
            {
                return this.Visibility != Visibility.Collapsed;
            }
            set
            {
                if (value) 
                { 
                    this.Visibility = Visibility.Visible; 
                }
                else 
                { 
                    this.Visibility = Visibility.Collapsed; 
                }
            }
        }
        #endregion

        #region public methods
        public FoodView(FoodModel foodModel)
        {
            InitializeComponent();
            _foodModel = foodModel;
            _foodModel.Origin = new Point(this.Width / 2, this.Height / 2);
        }

        public void Update()
        {
            IsVisible = _foodModel.IsVisible;
            RenderTransform = _foodModel.RenderTransform;
        }
        #endregion

        #region private methods

        #endregion

        #region eventhandlers
        #endregion

        #region events
        #endregion

        #region private variables
        FoodModel _foodModel;
        #endregion     
        
    }
}
