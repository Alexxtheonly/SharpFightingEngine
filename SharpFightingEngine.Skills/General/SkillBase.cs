using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.General
{
  public abstract class SkillBase : ISkill
  {
    protected static readonly Random Random = new Random();

    public abstract Guid Id { get; }

    public abstract string Name { get; }

    public int Damage => Random.Next(DamageLow, DamageHigh + 1);

    public abstract int DamageLow { get; }

    public abstract int DamageHigh { get; }

    public abstract float Range { get; }

    public abstract int Energy { get; }

    public abstract int Cooldown { get; }

    public virtual IEnumerable<EngineTick> Perform(IFighterStats actor, IFighterStats target, EngineCalculationValues calculationValues)
    {
      return Enumerable.Empty<EngineTick>();
    }

    protected IPosition CalculateKnockBackPosition(IPosition actor, IPosition target, float distance)
    {
      return actor.CalculateKnockBackPosition(target, distance);
    }

    protected IPosition CalculatePullPosition(IPosition actor, IPosition target, float distance)
    {
      return actor.CalculatePullPosition(target, distance);
    }
  }
}
