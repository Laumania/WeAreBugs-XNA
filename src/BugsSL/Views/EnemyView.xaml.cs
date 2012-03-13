using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BugsSL.Models;

namespace BugsSL.Views
{
    public partial class EnemyView : UserControl
    {
        #region public methods
        public EnemyView(EnemyModel enemyModel)
        {
            InitializeComponent();
            _enemyModel = enemyModel;
            _enemyModel.Origin = new Point(this.Width / 2, this.Height / 2);
        }
        
        public void Update()
        {
            RenderTransform = _enemyModel.RenderTransform;
            UpdateEnemyExcitement();
        }
        #endregion

        #region private methods
        private void UpdateEnemyExcitement()
        {
            int color = (int)(255 * _enemyModel.Excitement);
            this.Body.Fill = new SolidColorBrush(Color.FromArgb((byte)color, _excitementR, _excitementG, _excitementB));
        }
        #endregion

        #region private variables
        EnemyModel _enemyModel;
        private byte _excitementR = 220;
        private byte _excitementG = 0;
        private byte _excitementB = 0;
        #endregion     
    }
}
