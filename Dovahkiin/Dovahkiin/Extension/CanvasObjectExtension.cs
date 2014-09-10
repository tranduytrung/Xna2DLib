using System;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Extension
{
    public static class CanvasObjectExtension
    {
        public static double DistanceSquare(this ICanvasObject source, ICanvasObject target)
        {
            var dx = source.X - target.X;
            var dy = source.Y - target.Y;

            return dx * dx + dy * dy;
        }

        public static double Distance(this ICanvasObject source, ICanvasObject target)
        {
            return Math.Sqrt(source.DistanceSquare(target));
        }
    }
}
