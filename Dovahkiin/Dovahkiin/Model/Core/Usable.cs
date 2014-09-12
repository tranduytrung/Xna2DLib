namespace Dovahkiin.Model.Core
{
    public abstract class Usable : ICarriable
    {
        public abstract int ResouceId { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract int Value { get; }

        /// <summary>
        /// Apply the item effects to target
        /// </summary>
        /// <param name="target">the target to apply</param>
        /// <returns>return true if this item can applied to target. Otherwise, false</returns>
        internal bool Apply(ICreature target)
        {
            if (UsableTimes == 0) return false;

            var isSuccess = ApplyEffect(target);

            if (isSuccess && UsableTimes != int.MinValue)
                UsableTimes -= 1;

            return isSuccess;
        }

        /// <summary>
        /// Override this method to do effect on target
        /// </summary>
        /// <param name="target">the target to apply</param>
        /// <returns>return true if this item can applied to target. Otherwise, false</returns>
        protected abstract bool ApplyEffect(ICreature target);

        /// <summary>
        /// Number of times that this item can be used
        /// Set to int.Min denoted as forever
        /// </summary>
        public int UsableTimes { get; internal set; }
    }
}