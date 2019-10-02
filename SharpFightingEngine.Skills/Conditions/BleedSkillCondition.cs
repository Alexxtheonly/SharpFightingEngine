using System;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.Conditions
{
  public class BleedSkillCondition : SkillConditionBase
  {
    private const int Duration = 3;

    public BleedSkillCondition(IFighterStats source)
      : base(source)
    {
    }

    public override Guid Id => new Guid("F84C756E-CE75-4F65-9724-2F289231EC1B");

    public override string Name => "Bleed";

    public override bool PreventsPerformingActions => false;

    public override float? HealingReduced => null;

    public override int Remaining { get; set; } = Duration;

    public override int Initial => Duration;

    public override int Damage => 3;
  }
}
