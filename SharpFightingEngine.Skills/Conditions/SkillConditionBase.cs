using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Skills.Conditions
{
  public abstract class SkillConditionBase : ISkillCondition
  {
    public SkillConditionBase(IFighterStats source)
    {
      Source = source;
    }

    public abstract Guid Id { get; }

    public abstract string Name { get; }

    public abstract bool PreventsPerformingActions { get; }

    public abstract float? HealingReduced { get; }

    public abstract int Damage { get; }

    public abstract int Remaining { get; set; }

    public IFighterStats Source { get; }

    public virtual void Apply(IStats stats)
    {
    }

    public virtual IEnumerable<EngineTick> Apply(IFighterStats target, IFighterStats source, EngineCalculationValues calculationValues)
    {
      if (!target.IsAlive(calculationValues))
      {
        return Enumerable.Empty<EngineTick>();
      }

      var damage = source.GetConditionDamage(calculationValues, this);
      target.DamageTaken += damage;

      return new FighterConditionDamageTick()
      {
        Condition = this.AsStruct(),
        Damage = damage,
        Fighter = target.AsStruct(),
        Source = source.AsStruct(),
      }.Yield();
    }
  }
}
