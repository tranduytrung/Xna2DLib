using System;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Animation
{
    public class ColorAnimation : Animation<Color>
    {
        public Color? From { get; set; }
        private Color ActualFrom { get; set; }
        public Color To { get; set; }

        public Func<Color, double, Color> Function { get; set; }

        private static Color Add(Color l, Color r)
        {
            return new Color(l.R + r.R, l.G + r.G, l.B + r.B, l.A + r.A);
        }

        private static Color Subtract(Color l, Color r)
        {
            return new Color(l.R - r.R, l.G - r.G, l.B - r.B, l.A - r.A);
        }

        public override void Initialize()
        {
            if (From == null)
                ActualFrom = GetAccessor.Invoke();
            else
                ActualFrom = From.Value;
        }

        protected override Color AnimationFunction(double elapsedProportion)
        {
            return Add(Function.Invoke(Subtract(To, ActualFrom), elapsedProportion), ActualFrom);
        }

        public override object Clone()
        {
            var obj = new ColorAnimation(GetAccessor, SetAccessor)
            {
                AutoReverve = this.AutoReverve,
                From = this.From,
                To = this.To,
                BeginTime = this.BeginTime,
                Duration = this.Duration,
                RepeatDelay = this.RepeatDelay,
                RepeatTime = this.RepeatTime
            };

            return obj;
        }

        public static readonly Func<Color, double, Color> Linear =
            (param, elapsedProportion) =>
                new Color((int) (elapsedProportion*param.R), (int) (elapsedProportion*param.G),
                    (int) (elapsedProportion*param.B), (int) (elapsedProportion*param.A));

        public ColorAnimation(GetAccessorDelegate<Color> getAccessor, SetAccessorDelegate<Color> setAccessor)
            : base(getAccessor, setAccessor)
        {
            Function = Linear;
        }

        public ColorAnimation(object target, string propertyPath)
            : base(target, propertyPath)
        {
            Function = Linear;
        }
    }
}