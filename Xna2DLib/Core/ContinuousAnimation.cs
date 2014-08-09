using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tranduytrung.Xna.Core
{
    [Serializable]
    public class ContinuousAnimation : Animation
    {
        private double _from;
        private double _to;

        public double From
        {
            get { return _from; }
            set { _from = value; }
        }

        public double To
        {
            get { return _to; }
            set { _to = value; }
        }

        protected override object AnimationFunction(double elapsedProportion)
        {
            return elapsedProportion*(To - From) + From;
        }

        public override object Clone()
        {
            var obj = new ContinuousAnimation()
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
    }
}
