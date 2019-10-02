using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.Conditions
{
  public class CrippleSkillCondition : SkillConditionBase
  {
    private const int Duration = 2;

    public CrippleSkillCondition(IFighterStats source)
      : base(source)
    {
    }

    public override Guid Id => new Guid("7233ECE4-999D-45E5-8FF8-7D9AB4587DC9");

    public override string Name => "Cripple";

    public override bool PreventsPerformingActions => false;

    public override float? HealingReduced => null;

    public override int Damage => 0;

    public override int Remaining { get; set; } = Duration;

    public override int Initial => Duration;

    public override void Apply(IStats stats)
    {
      stats.Speed /= 2;
    }

    public override IEnumerable<EngineTick> Apply(IFighterStats target, IFighterStats source, EngineCalculationValues calculationValues)
    {
      return Enumerable.Empty<EngineTick>();
    }
  }
}
