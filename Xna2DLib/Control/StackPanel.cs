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
            int width, height;
            DesiredWidth = Width != int.MinValue ? Width : availableSize.Width;
            DesiredHeight = Height != int.MinValue ? Height : availableSize.Height;
            switch (Orientation)
            {
                case StackOrientation.Horizontal:
                    width = DesiredWidth;
                    height = 0;
                    foreach (var child in Children)
                    {
                        var margin = (Margin)child.GetValue(MarginProperty);
                        child.Measure(new Size(width - margin.Left - margin.Right, DesiredHeight - margin.Top - margin.Bottom));
                        width -= child.DesiredWidth + margin.Left + margin.Right;
                        height = Math.Max(child.DesiredHeight + margin.Top + margin.Bottom, height);

                        if (width >= 0) continue;

                        width = 0;
                        break;
                    }
                    DesiredWidth -= width;
                    if (height < DesiredHeight)
                        DesiredHeight = height;
                    break;
                case StackOrientation.Vertical:
                    height = DesiredHeight;
                    width = 0;
                    foreach (var child in Children)
                    {
                        var margin = (Margin)child.GetValue(MarginProperty);
                        child.Measure(new Size(DesiredWidth - margin.Left - margin.Right, height - margin.Top - margin.Bottom));
                        height -= child.DesiredHeight + margin.Top + margin.Bottom;
                        width = Math.Max(child.DesiredWidth + margin.Left + margin.Right, width);

                        if (height >= 0) continue;

                        height = 0;
                        break;
                    }
                    DesiredHeight -= height;
                    if (width < DesiredWidth)
                        DesiredWidth = width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            base.Arrange(finalRectangle);

            int width, height;
            var x = 0;
            var y = 0;
            switch (Orientation)
            {
                case StackOrientation.Horizontal:
                    width = finalRectangle.Width;
                    height = finalRectangle.Height;
                    foreach (var child in Children)
                    {
                        if (width <= 0)
                        {
                            child.Arrange(new Rectangle(0,0,0,0));
                            continue;
                        }

                        var margin = (Margin)child.GetValue(MarginProperty);

                        child.Arrange(AlignmentExtension.Align(child,
                            new Rectangle(x + margin.Left, y + margin.Top,
                                Math.Min(child.DesiredWidth, width - margin.Left - margin.Right),
                                height - margin.Top - margin.Bottom)));
                        x += margin.Left + child.DesiredWidth + margin.Right;
                        width -= child.DesiredWidth + margin.Left + margin.Right;
                    }
                    break;
                case StackOrientation.Vertical:
                    height = finalRectangle.Height;
                    width = finalRectangle.Width;
                    foreach (var child in Children)
                    {
                        if (height <= 0)
                        {
                            child.Arrange(new Rectangle(0,0,0,0));
                            continue;
                        }

                        var margin = (Margin)child.GetValue(MarginProperty);

                        child.Arrange(AlignmentExtension.Align(child,
                            new Rectangle(x + margin.Left, y + margin.Top,
                                width - margin.Left - margin.Right,
                                Math.Min(child.DesiredHeight, height - margin.Top - margin.Bottom))));
                        y += margin.Top + child.DesiredHeight + margin.Bottom;
                        height -= child.DesiredHeight + margin.Top + margin.Bottom;
                    }
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
