using System;
using System.Collections.Generic;

namespace PSR
{
    public class Wall
    {
        public Point FirstPoint { get; set; } = new Point();
        public Point SecondPoint { get; set; } = new Point();

        public override int GetHashCode()
        {
            int hash = 23;
            foreach (char c in ToString())
            {
                hash = hash * 31 + c;
            }
            return hash;
        }

        public override bool Equals(object o)
        {
            if (o == null || !(o is Wall)) return false;

            Wall w = (Wall)o;
            return this == w;
        }

        public static bool operator ==(Wall a, Wall b)
        {
            return
                (
                    (a.FirstPoint.Equals(b.FirstPoint) && (a.SecondPoint.Equals(b.SecondPoint)))
                    ||
                    (a.FirstPoint.Equals(b.SecondPoint) && (a.SecondPoint.Equals(b.FirstPoint)))
                );
        }

        public static bool operator !=(Wall a, Wall b)
        {
            return
               !(
                    (a.FirstPoint.Equals(b.FirstPoint) && (a.SecondPoint.Equals(b.SecondPoint)))
                    ||
                    (a.FirstPoint.Equals(b.SecondPoint) && (a.SecondPoint.Equals(b.FirstPoint)))
                );
        }

        public double Length
        {
            get
            {

                return FirstPoint.DistanceTo(SecondPoint);
            }
        }

        public List<Point> Points2D(double step)
        {
            List<Point> points = new List<Point>();

            if (FirstPoint.X == SecondPoint.X)
            {
                for (double y = Math.Min(FirstPoint.Y, SecondPoint.Y); y < Math.Max(FirstPoint.Y, SecondPoint.Y); y += step)
                {
                    points.Add(new Point() { X = FirstPoint.X, Y = y, Z = 0 });
                }
                points.Add(new Point() { X = FirstPoint.X, Y = Math.Max(FirstPoint.Y, SecondPoint.Y), Z = 0 });
                return points;
            }

            if (FirstPoint.Y == SecondPoint.Y)
            {
                for (double x = Math.Min(FirstPoint.X, SecondPoint.X); x < Math.Max(FirstPoint.X, SecondPoint.X); x += step)
                {
                    points.Add(new Point() { X = x, Y = FirstPoint.Y, Z = 0 });
                }
                points.Add(new Point() { X = Math.Max(FirstPoint.X, SecondPoint.X), Y = FirstPoint.Y, Z = 0 });
                return points;
            }

            double k = (FirstPoint.Y - SecondPoint.Y) / (FirstPoint.X - SecondPoint.X);
            double b = SecondPoint.X * k - SecondPoint.Y;

            points.Add(new Point() { X = Math.Min(FirstPoint.X, SecondPoint.X), Y = (FirstPoint.X < SecondPoint.X) ? FirstPoint.Y : SecondPoint.Y, Z = 0 });
            for (double x = Math.Min(FirstPoint.X, SecondPoint.X) + step; x < Math.Max(FirstPoint.X, SecondPoint.X); x += step)
            {
                points.Add(new Point() { X = x, Y = x * k - b, Z = 0 });
            }
            points.Add(new Point() { X = Math.Max(FirstPoint.X, SecondPoint.X), Y = (FirstPoint.X > SecondPoint.X) ? FirstPoint.Y : SecondPoint.Y, Z = 0 });

            return points;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", FirstPoint.ToString(), SecondPoint.ToString());
        }
    }
}
