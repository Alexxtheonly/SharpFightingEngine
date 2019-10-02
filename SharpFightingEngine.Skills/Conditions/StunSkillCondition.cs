using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.Conditions
{
  public class StunSkillCondition : SkillConditionBase
  {
    private const int Duration = 2;

    public StunSkillCondition(IFighterStats source)
      : base(source)
    {
    }

    public override Guid Id => new Guid("09FBD6B5-3F0D-4DE5-B40B-D7D9C7AA7A92");

    public override string Name => "Stun";

    public override bool PreventsPerformingActions => true;

    public override float? HealingReduced => null;

    public override int Damage => 0;

    public override int Remaining { get; set; } = Duration;

    public override int Initial => Duration;

    public override IEnumerable<EngineTick> Apply(IFighterStats target, IFighterStats source, EngineCalculationValues calculationValues)
    {
      return base.Apply(target, source, calculationValues);
    }
  }
}
