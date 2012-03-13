using System;
using System.Windows;

namespace BugsSL
{
    public static class Mathematics
    {
        public static Point Subtract(Point p1, Point p2) 
        { 
            return new Point(p1.X - p2.X, p1.Y - p2.Y); 
        }
        
        public static Point Add(Point p1, Point p2) 
        { 
            return new Point(p1.X + p2.X, p1.Y + p2.Y); 
        }
        
        public static Point Multiply (Point p, float scale) 
        {
            return new Point(p.X * scale, p.Y * scale); 
        }
        
        public static float Length(Point p) 
        {
            double num = (p.X * p.X) + (p.Y * p.Y); return (float)Math.Sqrt(num); 
        }
        
        public static Point Normalize(Point p)
        {
            Point po = new Point();
            double num2 = (p.X * p.X) + (p.Y * p.Y);
            double num = 1f / ((float)Math.Sqrt(num2));
            po.X = p.X * num;
            po.Y = p.Y * num;
            return po;
        }

        public static float Distance(Point p1, Point p2)
        {
            return Length(Subtract(p1, p2));
        }

        public static Point Lerp(Point from, Point to, float amount)
        {
            Point point = new Point();
            point.X = from.X + ((to.X - from.X) * amount);
            point.Y = from.Y + ((to.Y - from.Y) * amount);
            return point;
        }

        public static double Lerp(double from, double to, double amount)
        {
            return from + ((to - from) * amount);
        }

        public static double PointToAngle(Point p)
        {
            double r = (double)Math.Atan2(p.X, -(double)p.Y);
            double a = r * 57.29f;
            return a;
        }

        public static Point Truncate(Point x, float maxLength)
        {
            Point pn = x;
            float l = Length(x);
            l = Math.Min(l, maxLength);
            if (l > 0)
            {
                pn = Normalize(x);
            }
            x = Multiply(pn, l);
            return x;
        }
    }
}
