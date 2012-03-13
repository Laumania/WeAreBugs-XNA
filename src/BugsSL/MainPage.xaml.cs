using System.Windows;
using BugsSL.Controllers;
using Microsoft.Phone.Controls;

namespace BugsSL
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Controller _controller;

        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        public Point GetMousePosition()
        {
            return new Point(3, 3);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _controller = new Controller(this);
            _controller.Initialize();
        }
    }
}