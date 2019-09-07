using System;

namespace SharpFightingEngine.Skills.General
{
  public abstract class SkillBase : ISkill
  {
    private static readonly Random Random = new Random();

    public abstract Guid Id { get; }

    public abstract string Name { get; }

    public int Damage => Random.Next(DamageLow, DamageHigh + 1);

    public abstract int DamageLow { get; }

    public abstract int DamageHigh { get; }

    public abstract float Range { get; }

    public abstract int Energy { get; }
  }
}
