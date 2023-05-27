using System;
using static System.Net.Mime.MediaTypeNames;

namespace PSR
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

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
            if (o == null || !(o is Point)) return false;

            Point p = (Point)o; 
            return DistanceTo(p) == 0;
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.DistanceTo(b) == 0;
        }
        
        public static bool operator !=(Point a, Point b)
        {
            return a.DistanceTo(b) != 0;
        }

        public double DistanceTo(Point p)
        {
            return Math.Sqrt((X - p.X) * (X - p.X)
                             + (Y - p.Y) * (Y - p.Y)
                             + (Z - p.Z) * (Z - p.Z));
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }
    }
}
