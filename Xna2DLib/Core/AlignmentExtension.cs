using System;
using Microsoft.Xna.Framework;

namespace tranduytrung.Xna.Core
{
    public static class AlignmentExtension
    {
        public static readonly AttachableProperty HorizontalAlignmentProperty = AttachableProperty.RegisterProperty(typeof(HorizontalAlignment), HorizontalAlignment.Left);
        public static readonly AttachableProperty VerticalAlignmentProperty = AttachableProperty.RegisterProperty(typeof(VerticalAlignment), VerticalAlignment.Top);
        public static Rectangle Align(DrawableObject element, Rectangle availableRect)
        {
            int x, y, width, height;
            var hAlignment = (HorizontalAlignment)element.GetValue(HorizontalAlignmentProperty);
            var vAlignment = (VerticalAlignment)element.GetValue(VerticalAlignmentProperty);

            if (availableRect.Width <= element.DesiredWidth)
            {
                x = availableRect.X;
                width = availableRect.Width;
            }
            else
            {
                switch (hAlignment)
                {
                    case HorizontalAlignment.Left:
                        x = availableRect.X;
                        width = element.DesiredWidth;
                        break;
                    case HorizontalAlignment.Right:
                        x = availableRect.X + availableRect.Width - element.DesiredWidth;
                        width = element.DesiredWidth;
                        break;
                    case HorizontalAlignment.Center:
                        x = availableRect.X + (availableRect.Width - element.DesiredWidth) / 2;
                        width = element.DesiredWidth;
                        break;
                    case HorizontalAlignment.Stretch:
                        x = availableRect.X;
                        width = availableRect.Width;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (availableRect.Height <= element.DesiredHeight)
            {
                y = availableRect.Y;
                height = availableRect.Height;
            }
            else
            {
                switch (vAlignment)
                {
                    case VerticalAlignment.Top:
                        y = availableRect.Y;
                        height = element.DesiredHeight;
                        break;
                    case VerticalAlignment.Bottom:
                        y = availableRect.Y + availableRect.Height - element.DesiredHeight;
                        height = element.DesiredHeight;
                        break;
                    case VerticalAlignment.Center:
                        y = availableRect.Y + (availableRect.Height - element.DesiredHeight) / 2;
                        height = element.DesiredHeight;
                        break;
                    case VerticalAlignment.Stretch:
                        y = availableRect.Y;
                        height = availableRect.Height;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new Rectangle(x, y, width, height);
        }
    }


    public enum HorizontalAlignment
    {
        Left, Right, Center, Stretch
    }

    public enum VerticalAlignment
    {
        Top, Bottom, Center, Stretch
    }
}
