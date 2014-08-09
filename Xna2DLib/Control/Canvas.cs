using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Control
{
    public class Canvas : Panel
    {
        public static readonly AttachableProperty XProperty = AttachableProperty.RegisterProperty(typeof (int));
        public static readonly AttachableProperty YProperty = AttachableProperty.RegisterProperty(typeof(int));

        public override void Measure(Size availableSize)
        {
            foreach (var child in Children)
            {
                child.Measure(availableSize);
            }

            base.Measure(availableSize);
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            foreach (var child in Children)
            {
                var childX = (int)child.GetValue(Canvas.XProperty);
                var childY = (int) child.GetValue(Canvas.YProperty);
                child.Arrange(new Rectangle(childX, childY, child.DesiredWidth, child.DesiredHeight));
            }

            base.Arrange(finalRectangle);
        }
    }
}
