using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.Buffs
{
  public class ReflectSkillBuff : ISkillBuff
  {
    public float? ReflectChance => 66.6F;

    public Guid Id => new Guid("1F13F311-3759-46D7-B6A2-7552AC18237B");

    public string Name => "Reflect";

    public int Remaining { get; set; }

    public int Initial => 2;

    public IFighterStats Source { get; set; }

    public void Apply(IStats stats)
    {
    }

    public IEnumerable<EngineTick> Apply(IFighterStats target, IFighterStats source, EngineCalculationValues calculationValues)
    {
      return Enumerable.Empty<EngineTick>();
    }
  }
}
