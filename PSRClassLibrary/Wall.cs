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

        public override string ToString()
        {
            return string.Format("{0} - {1}", FirstPoint.ToString(), SecondPoint.ToString());
        }
    }
}
