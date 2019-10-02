using System;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.Conditions
{
  public class PoisonSkillCondition : SkillConditionBase
  {
    private const int Duration = 3;

    public PoisonSkillCondition(IFighterStats source)
      : base(source)
    {
    }

    public override Guid Id => new Guid("2E2767F9-A7D9-4D67-B0FC-D6A1B6F258C8");

    public override string Name => "Poison";

    public override bool PreventsPerformingActions => false;

    public override float? HealingReduced => 0.66F;

    public override int Damage => 5;

    public override int Remaining { get; set; } = Duration;

    public override int Initial => Duration;
  }
}
