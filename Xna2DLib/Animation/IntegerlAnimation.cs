using System;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Animation
{
    public class IntegerlAnimation : Animation<int>
    {
        public int? From { get; set; }
        private int ActualFrom { get; set; }
        public int To { get; set; }

        public Func<int, double, int> Function { get; set; }

        public override void Initialize()
        {
            if (From == null)
                ActualFrom = (int)GetAccessor.Invoke();
            else
                ActualFrom = From.Value;
        }

        protected override int AnimationFunction(double elapsedProportion)
        {
            return Function.Invoke(To - ActualFrom, elapsedProportion) + ActualFrom;
        }

        public override object Clone()
        {
            var obj = new IntegerlAnimation(GetAccessor, SetAccessor)
            {
                AutoReverve = this.AutoReverve,
                BeginTime = this.BeginTime,
                Duration = this.Duration,
                RepeatDelay = this.RepeatDelay,
                RepeatTime = this.RepeatTime
            };

            return obj;
        }


        public static readonly Func<int, double, int> Linear =
            (param, elapsedProportion) => (int)(elapsedProportion*param);

        public static readonly Func<int, double, int> Sine =
            (param, elapsedProportion) => (int)(Math.Sin(elapsedProportion*Math.PI) * param);

        public static readonly Func<int, double, int> Cosine =
            (param, elapsedProportion) => (int)(Math.Cos(elapsedProportion * Math.PI) * param);

        public IntegerlAnimation(GetAccessorDelegate<int> getAccessor, SetAccessorDelegate<int> setAccessor) 
            : base(getAccessor, setAccessor)
        {
            Function = Linear;
        }

        public IntegerlAnimation(object target, string propertyPath) : base(target, propertyPath)
        {
            Function = Linear;
        }
    }
}