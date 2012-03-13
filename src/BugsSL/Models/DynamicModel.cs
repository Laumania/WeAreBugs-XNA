using System.Windows;
using System.Windows.Media;

namespace BugsSL.Models
{
    public class DynamicModel
    {
        #region properties
        public TransformGroup RenderTransform { get; private set; }

        public Point Position
        {
            get { return _position; }
            set
            {
                _position.X = value.X;
                _position.Y = value.Y;
                if (_isInitialized)
                {
                    SetTranslationMatrix();
                }
            }
        }

        public double Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                if (_isInitialized)
                {
                    SetScaleTransform();
                }
            }
        }

        public Point Front
        {
            get {return _front;}
            set
            {
                _front = value;
                if(_isInitialized)
                {
                    SetRotationMatrix();
                }
            }
        }

        public Point Origin
        {
            get { return _origin; }
            set
            {
                _origin = value;
                if (_isInitialized)
                {
                    SetOriginMatrix();
                }
            }
        }

        #endregion

        #region public methods
        public DynamicModel()
        {
            //defaults
            Front = new Point(0, -1);
            Position = new Point(0, 0);
            Scale = 1;
            Origin = new Point(0, 0);
        }

        public virtual void Initialize()
        {
            if (_isInitialized) return;
            _originMatrix = Matrix.Identity;// new Matrix(1, 0, 0, 1, 0, 0);            
            _rotationMatrix = Matrix.Identity;// new Matrix(1, 0, 0, 1, 0, 0);
            _translationMatrix = Matrix.Identity;//new Matrix(1, 0, 0, 1, 0, 0);

            _originTransform = new MatrixTransform();
            _rotateTransform = new MatrixTransform();
            _translateTransform = new MatrixTransform();
            _scaleTransform = new ScaleTransform();

            SetOriginMatrix();
            SetRotationMatrix();           
            SetTranslationMatrix();
            SetScaleTransform();

            RenderTransform = new TransformGroup();
            RenderTransform.Children.Add(_scaleTransform);
            RenderTransform.Children.Add(_originTransform);
            RenderTransform.Children.Add(_rotateTransform);
            RenderTransform.Children.Add(_translateTransform);
            _isInitialized = true;
       }

        public void SetOriginMatrix()
        {
            _originMatrix.OffsetX = -_origin.X * _scale;
            _originMatrix.OffsetY = -_origin.Y * _scale;
            _originTransform.Matrix = _originMatrix;
        }

        private void SetTranslationMatrix()
        {
            _translationMatrix.OffsetX = _position.X;
            _translationMatrix.OffsetY = _position.Y;
            _translateTransform.Matrix = _translationMatrix;
        }

        private void SetScaleTransform()
        {
            _scaleTransform.ScaleX = _scale;
            _scaleTransform.ScaleY = _scale;
        }

        private void SetRotationMatrix()
        {
            _rotationMatrix.M11 = _front.X;
            _rotationMatrix.M12 = _front.Y;
            _rotationMatrix.M21 = -_front.Y;
            _rotationMatrix.M22 = _front.X;
            _rotateTransform.Matrix = _rotationMatrix;
        }

        #endregion

        #region private methods
        #endregion

        #region eventhandlers
        #endregion

        #region events
        #endregion

        #region private variables
        private MatrixTransform _originTransform;
        private MatrixTransform _rotateTransform;
        private MatrixTransform _translateTransform;
        private ScaleTransform _scaleTransform;

        Point _position;
        Point _origin;
        Point _front;
        double _scale;
        Matrix _originMatrix, _rotationMatrix, _translationMatrix;
        bool _isInitialized = false;
        #endregion        





    }
}
