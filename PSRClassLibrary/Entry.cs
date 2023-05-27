namespace PSR
{
    public class Entry
    {
        public Point Center { get; set; } = new Point();
        public double Diameter { get; set; } = .0;
        public override string ToString()
        {
            return string.Format("{0}, D={1}", Center.ToString(), Diameter);
        }
    }
}
