﻿namespace Dovahkiin.Model.Core
{
    public interface ICreature
    {
        int ResouceId { get; }
        int HitPoint { get; }
        int HitGauge { get; }
        int MagicPoint { get; }
        int MagicGauge { get; }
        int Level { get; }
        int AccumulatedExperience { get; }
        int ExperienceGauge { get; }
        int Power { get; }
    }
}
