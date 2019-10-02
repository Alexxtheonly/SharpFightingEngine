using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.Conditions
{
  public struct SkillCondition : ISkillCondition
  {
    public bool PreventsPerformingActions { get; set; }

    public float? HealingReduced { get; set; }

    public int Damage { get; set; }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Remaining { get; set; }

    public int Initial { get; set; }

    public IFighterStats Source { get; set; }

    public void Apply(IStats stats)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<EngineTick> Apply(IFighterStats target, IFighterStats source, EngineCalculationValues calculationValues)
    {
      throw new NotImplementedException();
    }
  }
}
