using System;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Control
{
    public class StackPanel : Panel
    {
        public StackOrientation Orientation { get; set; }

        public override void Measure(Size availableSize)
        {
            int orientSize;
            DesiredWidth = Width != int.MinValue ? Width : availableSize.Width;
            DesiredHeight = Height != int.MinValue ? Height : availableSize.Height;
            switch (Orientation)
            {
                case StackOrientation.Horizontal:
                    orientSize = DesiredWidth;
                    foreach (var child in Children)
                    {
                        var margin = (Margin)child.GetValue(Panel.MarginProperty);
                        child.Measure(new Size(orientSize - margin.Left - margin.Right, DesiredHeight - margin.Top - margin.Bottom));
                        orientSize -= child.DesiredWidth + margin.Left + margin.Right;

                        if (orientSize < 0)
                        {
                            orientSize = 0;
                        }
                    }
                    DesiredWidth -= orientSize;
                    break;
                case StackOrientation.Vertical:
                    orientSize = DesiredHeight;
                    foreach (var child in Children)
                    {
                        var margin = (Margin)child.GetValue(Panel.MarginProperty);
                        child.Measure(new Size(DesiredWidth - margin.Left - margin.Right, orientSize - margin.Top - margin.Bottom));
                        orientSize -= child.DesiredHeight + margin.Top + margin.Bottom;

                        if (orientSize < 0)
                        {
                            orientSize = 0;
                        }
                    }
                    DesiredHeight -= orientSize;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            base.Arrange(finalRectangle);

            int orientSize;
            var x = 0;
            var y = 0;
            switch (Orientation)
            {
                case StackOrientation.Horizontal:
                    orientSize = finalRectangle.Width;
                    foreach (var child in Children)
                    {
                        if (orientSize <= 0)
                        {
                            child.Arrange(new Rectangle(0,0,0,0));
                            continue;
                        }

                        var margin = (Margin)child.GetValue(Panel.MarginProperty);
                        child.Arrange(Align(child,
                            new Rectangle(x + margin.Left, y + margin.Top,
                                Math.Min(child.DesiredWidth, orientSize - margin.Left - margin.Right),
                                finalRectangle.Height - margin.Top - margin.Bottom)));
                        x = margin.Left + child.DesiredWidth + margin.Right;
                        orientSize -= child.DesiredWidth + margin.Left + margin.Right;
                    }
                    ActualWidth -= orientSize < 0? 0: orientSize;
                    break;
                case StackOrientation.Vertical:
                    orientSize = finalRectangle.Height;
                    foreach (var child in Children)
                    {
                        if (orientSize <= 0)
                        {
                            child.Arrange(new Rectangle(0,0,0,0));
                            continue;
                        }

                        var margin = (Margin)child.GetValue(Panel.MarginProperty);
                        child.Arrange(Align(child,
                            new Rectangle(x + margin.Left, y + margin.Top,
                                finalRectangle.Width - margin.Left - margin.Right,
                                Math.Min(child.DesiredHeight, orientSize - margin.Top - margin.Bottom))));
                        y = margin.Top + child.DesiredHeight + margin.Bottom;
                        orientSize -= child.DesiredHeight + margin.Top + margin.Bottom;
                    }
                    ActualHeight -= orientSize < 0? 0: orientSize;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum StackOrientation
    {
        Horizontal,
        Vertical
    }
}
