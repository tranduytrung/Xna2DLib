using System;

namespace tranduytrung.Xna.Core
{
    public class DoubleAnimation : Animation<double>
    {
        public double? From { get; set; }
        private double ActualFrom { get; set; }
        public double To { get; set; }

        public Func<double, double, double> Function { get; set; }

        public override void Initialize()
        {
            if (From == null)
                ActualFrom = (double)GetAccessor.Invoke();
            else
                ActualFrom = From.Value;
        }

        protected override double AnimationFunction(double elapsedProportion)
        {
            return Function.Invoke(To - ActualFrom, elapsedProportion) + ActualFrom;
        }

        public override object Clone()
        {
            var obj = new DoubleAnimation(GetAccessor, SetAccessor)
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

        public static readonly Func<double, double, double> Linear =
            (param, elapsedProportion) => elapsedProportion*param;

        public static readonly Func<double, double, double> Sine =
            (param, elapsedProportion) => Math.Sin(elapsedProportion*Math.PI) * param;

        public static readonly Func<double, double, double> Cosine =
            (param, elapsedProportion) => Math.Cos(elapsedProportion * Math.PI) * param;

        public DoubleAnimation(GetAccessorDelegate<double> getAccessor, SetAccessorDelegate<double> setAccessor)
            : base(getAccessor, setAccessor)
        {
            Function = Linear;
        }

        public DoubleAnimation(object target, string propertyPath) : base(target, propertyPath)
        {
            Function = Linear;
        }
    }
}
