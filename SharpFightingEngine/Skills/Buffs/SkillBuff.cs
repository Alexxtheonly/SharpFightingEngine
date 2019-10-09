using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.Buffs
{
  public struct SkillBuff : ISkillBuff
  {
    public float? ReflectChance { get; set; }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Remaining { get; set; }

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
