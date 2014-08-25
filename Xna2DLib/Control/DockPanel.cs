using System;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Control
{
    public class DockPanel : Panel
    {
        public static AttachableProperty DockProperty = AttachableProperty.RegisterProperty(typeof (Dock), Dock.Left);

        public bool AutoFillLastChild { get; set; }

        public override void Measure(Size availableSize)
        {
            var width = DesiredWidth = Width != int.MinValue ? Width : availableSize.Width;
            var height = DesiredHeight = Height != int.MinValue ? Height : availableSize.Height;

            foreach (var child in Children)
            {
                var margin = (Margin)child.GetValue(MarginProperty);
                var widthMargin = margin.Left + margin.Right;
                var heightMargin = margin.Bottom + margin.Top;
                child.Measure(new Size(width - widthMargin, height - heightMargin));
                switch ((Dock)child.GetValue(DockProperty))
                {
                    case Dock.Top:
                    case Dock.Bottom:
                        height -= child.DesiredHeight + heightMargin;
                        break;
                    case Dock.Left:
                    case Dock.Right:
                        width -= child.DesiredWidth + widthMargin;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override void Arrange(Rectangle finalRectangle)
        {
            base.Arrange(finalRectangle);

            var rect = new Rectangle(0, 0, finalRectangle.Width, finalRectangle.Height);
            var n = AutoFillLastChild ? Children.Count - 1 : Children.Count;
            for (var i = 0; i < n; i++)
            {
                var child = Children[i];
                var margin = (Margin)child.GetValue(MarginProperty);
                Rectangle childRect;

                switch ((Dock)child.GetValue(DockProperty))
                {
                    case Dock.Top:
                        childRect = AlignmentExtension.Align(child,
                            new Rectangle(rect.X + margin.Left, rect.Y + margin.Top,
                                rect.Width - margin.Left - margin.Right,
                                Math.Min(rect.Height - margin.Top - margin.Bottom, child.DesiredHeight)));
                        child.Arrange(childRect);

                        rect.Height -= margin.Top + childRect.Height + margin.Bottom;
                        rect.Y += margin.Top + childRect.Height + margin.Bottom;
                        break;
                    case Dock.Bottom:
                        childRect = AlignmentExtension.Align(child,
                            new Rectangle(rect.X + margin.Left,
                                rect.Y + margin.Top - margin.Bottom + Math.Max(rect.Height - child.DesiredHeight, 0),
                                rect.Width - margin.Left - margin.Right,
                                Math.Min(rect.Height - margin.Top - margin.Bottom, child.DesiredHeight)));

                        child.Arrange(childRect);

                        rect.Height -= margin.Top + childRect.Height + margin.Bottom;
                        break;
                    case Dock.Left:
                        childRect = AlignmentExtension.Align(child,
                            new Rectangle(rect.X + margin.Left, rect.Y + margin.Top,
                                Math.Min(rect.Width - margin.Left - margin.Right, child.DesiredWidth),
                                rect.Height - margin.Top - margin.Bottom));
                        child.Arrange(childRect);

                        rect.Width -= margin.Left + childRect.Width + margin.Right;
                        rect.X += margin.Left + childRect.Width + margin.Right;
                        break;
                    case Dock.Right:
                        childRect = AlignmentExtension.Align(child,
                            new Rectangle(rect.X + margin.Left - margin.Right + Math.Max(rect.Width - child.DesiredWidth, 0), rect.Y + margin.Top,
                                Math.Min(rect.Width - margin.Left - margin.Right, child.DesiredWidth),
                                rect.Height - margin.Top - margin.Bottom));
                        child.Arrange(childRect);

                        rect.Width -= margin.Left + childRect.Width + margin.Right;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (AutoFillLastChild)
            {
                var child = Children[Children.Count - 1];
                var margin = (Margin)child.GetValue(MarginProperty);
                child.Arrange(AlignmentExtension.Align(child,
                    new Rectangle(rect.X + margin.Left, rect.Y + margin.Top, rect.Width - margin.Left - margin.Right,
                        rect.Height - margin.Top - margin.Bottom)));
            }
        }
    }

    public enum Dock
    {
        Bottom, Left, Right, Top
    }
}
