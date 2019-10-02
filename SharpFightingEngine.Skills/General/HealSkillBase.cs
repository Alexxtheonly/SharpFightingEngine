using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.General
{
  public abstract class HealSkillBase : IHealSkill
  {
    public abstract Guid Id { get; }

    public abstract string Name { get; }

    public abstract int Heal { get; }

    public abstract float Range { get; }

    public abstract int Cooldown { get; }

    public virtual IEnumerable<EngineTick> Perform(IFighterStats actor, IFighterStats target, EngineCalculationValues calculationValues)
    {
      return Enumerable.Empty<EngineTick>();
    }
  }
}
