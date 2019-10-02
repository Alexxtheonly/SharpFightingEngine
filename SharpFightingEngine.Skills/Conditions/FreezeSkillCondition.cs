using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.Conditions
{
  public class FreezeSkillCondition : SkillConditionBase
  {
    private const int Duration = 1;

    public FreezeSkillCondition(IFighterStats source)
      : base(source)
    {
    }

    public override Guid Id => new Guid("8DDD3B8E-25A4-46C2-BCCE-031C21CB396E");

    public override string Name => "Freeze";

    public override bool PreventsPerformingActions => true;

    public override float? HealingReduced => null;

    public override int Damage => 0;

    public override int Remaining { get; set; }

    public override int Initial => Duration;

    public override IEnumerable<EngineTick> Apply(IFighterStats target, IFighterStats source, EngineCalculationValues calculationValues)
    {
      return Enumerable.Empty<EngineTick>();
    }
  }
}
