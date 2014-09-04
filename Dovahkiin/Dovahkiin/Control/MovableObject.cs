using tranduytrung.Xna.Core;

namespace Dovahkiin.Control
{
    public class MovableObject : ContentPresenter
    {
        public int CellWidth { get; set; }
        public int CellHeight { get; set; }

        public double CellRelativeX { get; private set; }
        public double CellRelativeY { get; private set; }

        public void SetRelativePosition(double x, double y)
        {
            CellRelativeX = x;
            CellRelativeY = y;

            Transform.TranslateX = (int)(CellWidth*x);
            Transform.TranslateY = (int)(CellHeight*y);
        }

        public MovableObject()
        {
            Transform = new Transfrormation();
            Transform.OriginY = 1;
        }
    }
}
