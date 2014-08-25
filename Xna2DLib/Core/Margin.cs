namespace tranduytrung.Xna.Core
{
    public class Margin
    {
        public int Left { get; set; }

        public int Top { get; set; }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public Margin(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Margin(int leftRight, int topBottom) : this(leftRight, topBottom, leftRight, topBottom)
        {
        }

        public Margin(int margin) : this(margin, margin, margin, margin)
        {
        }
    }
}
