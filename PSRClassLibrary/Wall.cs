namespace PSR
{
    public class Wall
    {
        public Point FirstPoint { get; set; } = new Point();
        public Point SecondPoint { get; set; } = new Point();
        public override string ToString()
        {
            return string.Format("{0} - {1}", FirstPoint.ToString(), SecondPoint.ToString());
        }
    }
}
