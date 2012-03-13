using System;
using System.Windows;
using System.Windows.Controls;
using BugsSL.Models;

namespace BugsSL.Views
{
    public partial class BugView : UserControl
    {
        BugModel _bugModel;

        public BugView(BugModel bugModel)
        {
            InitializeComponent();
            _bugModel = bugModel;
            _bugModel.Origin = new Point(this.Width / 2, this.Height / 2);
        }

        public void Update()
        {
            MotionTrailGradientStop.Offset = Math.Max(.3f, Mathematics.Length(_bugModel.Velocity) / _bugModel.MaxSpeed);
            RenderTransform = _bugModel.RenderTransform;
        }
    }
}
