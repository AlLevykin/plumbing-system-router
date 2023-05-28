using System.Collections.Generic;

namespace PSR
{
    public class Module
    {
        public IList<Wall> Walls   = new List<Wall>();
        public IList<Entry> Drains = new List<Entry>();
        public Entry VentStack { get; set; } = new Entry();
        // Результаты
        public double tubeLength = 0;
        public List<Point> errors = new List<Point>();
        public List<Point> tripls = new List<Point>();
        public List<Point> angles90 = new List<Point>();
        public List<Point> angles30 = new List<Point>();
        public List<Point> angles45 = new List<Point>();
        public List<Point> sockets = new List<Point>();
        public List<Point> crosses = new List<Point>();
    }
}
