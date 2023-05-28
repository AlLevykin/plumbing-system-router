using QuickGraph;
using System;
using System.Reflection;
using System.Security.Cryptography;

namespace PSR.Helpers
{
    public class Geometry
    {
        internal static double AngleBetween(IEdge<Point> edge1, IEdge<Point> edge2)
        {
            Point basePoint = (edge1.Source == edge2.Source) ? edge1.Source : edge1.Target; 
            Point p1 = (edge1.Source == basePoint) ? edge1.Target : edge1.Source;
            Point p2 = (edge2.Source == basePoint) ? edge2.Target : edge2.Source;
            return GetAngle(basePoint, p1, p2);
        }

        public static double GetAngle(Point basePoint, Point p1, Point p2)
        {
            double m1 = Math.Sqrt(Math.Pow(p1.X - basePoint.X, 2) + Math.Pow(p1.Y - basePoint.Y, 2) + Math.Pow(p1.Z - basePoint.Z, 2));
            double m2 = Math.Sqrt(Math.Pow(p2.X - basePoint.X, 2) + Math.Pow(p2.Y - basePoint.Y, 2) + Math.Pow(p2.Z - basePoint.Z, 2));
            double sm = (p1.X - basePoint.X) * (p2.X - basePoint.X) + (p1.Y - basePoint.Y) * (p2.Y - basePoint.Y) + (p1.Z - basePoint.Z) * (p2.Z - basePoint.Z);
            double cos = sm / (m1 * m2);
            return Math.Round(Math.Acos(cos) * 180 / Math.PI);
        }
    }
}
