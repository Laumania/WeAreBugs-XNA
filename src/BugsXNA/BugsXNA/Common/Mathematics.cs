using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace BugsXNA.Common
{
    public static class Mathematics
    {
        public static Vector2 Subtract(Vector2 p1, Vector2 p2)
        {
            return new Vector2(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Vector2 Add(Vector2 p1, Vector2 p2)
        {
            return new Vector2(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Vector2 Multiply(Vector2 p, float scale)
        {
            return new Vector2(p.X * scale, p.Y * scale);
        }

        public static float Length(Vector2 p)
        {
            double num = (p.X * p.X) + (p.Y * p.Y); return (float)Math.Sqrt(num);
        }

        public static Vector2 Normalize(Vector2 p)
        {
            var po = new Vector2();
            float num2 = (p.X * p.X) + (p.Y * p.Y);
            float num = 1f / ((float)Math.Sqrt(num2));
            po.X = p.X * num;
            po.Y = p.Y * num;
            return po;
        }

        public static float Distance(Vector2 p1, Vector2 p2)
        {
            return Length(Subtract(p1, p2));
        }

        public static Vector2 Lerp(Vector2 from, Vector2 to, float amount)
        {
            Vector2 point = new Vector2();
            point.X = from.X + ((to.X - from.X) * amount);
            point.Y = from.Y + ((to.Y - from.Y) * amount);
            return point;
        }

        public static float Lerp(float from, float to, float amount)
        {
            return from + ((to - from) * amount);
        }

        public static double PointToAngle(Vector2 p)
        {
            double r = (double)Math.Atan2(p.X, -(double)p.Y);
            double a = r * 57.29f;
            return a;
        }

        public static Vector2 Truncate(Vector2 x, float maxLength)
        {
            Vector2 pn = x;
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
