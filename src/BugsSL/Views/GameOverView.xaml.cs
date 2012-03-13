using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace BugsSL.Views
{
    public partial class GameOverView : UserControl
    {
        public event EventHandler Clicked;

        public GameOverView()
        {
            InitializeComponent();
        }

        private void LayoutRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Clicked != null) Clicked(this, null);
        }    
    }
}
