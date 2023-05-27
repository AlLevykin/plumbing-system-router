using System.Collections.Generic;

namespace PSR
{
    public class Module
    {
        public IList<Wall> Walls   = new List<Wall>();
        public IList<Entry> Drains = new List<Entry>();
        public Entry VentStack { get; set; } = new Entry();
    }
}
