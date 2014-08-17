using System;

namespace tranduytrung.Xna.Core
{
    public class FloatAnimation : Animation<float>
    {
        public float? From { get; set; }
        private float ActualFrom { get; set; }
        public float To { get; set; }

        public Func<float, double, float> Function { get; set; }

        public override void Initialize()
        {
            if (From == null)
                ActualFrom = (float)GetAccessor.Invoke();
            else
                ActualFrom = From.Value;
        }

        protected override float AnimationFunction(double elapsedProportion)
        {
            return Function.Invoke(To - ActualFrom, elapsedProportion) + ActualFrom;
        }

        public override object Clone()
        {
            var obj = new FloatAnimation(GetAccessor, SetAccessor)
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

        public static readonly Func<float, double, float> Linear =
            (param, elapsedProportion) => (float) (elapsedProportion * param);

        public static readonly Func<float, double, float> Sine =
            (param, elapsedProportion) => (float) (Math.Sin(elapsedProportion * Math.PI) * param);

        public static readonly Func<float, double, float> Cosine =
            (param, elapsedProportion) => (float) (Math.Cos(elapsedProportion * Math.PI) * param);

        public FloatAnimation(GetAccessorDelegate<float> getAccessor, SetAccessorDelegate<float> setAccessor)
            : base(getAccessor, setAccessor)
        {
            Function = Linear;
        }

        public FloatAnimation(object target, string propertyPath)
            : base(target, propertyPath)
        {
            Function = Linear;
        }
    }

}
