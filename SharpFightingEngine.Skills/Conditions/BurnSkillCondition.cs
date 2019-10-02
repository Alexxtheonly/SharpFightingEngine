using System;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.Conditions
{
  public class BurnSkillCondition : SkillConditionBase
  {
    private const int Duration = 2;

    public BurnSkillCondition(IFighterStats source)
      : base(source)
    {
    }

    public override Guid Id => new Guid("4650A573-8AE1-4FA6-AE3C-CA4346B7E31C");

    public override string Name => "Burn";

    public override bool PreventsPerformingActions => false;

    public override float? HealingReduced => null;

    public override int Damage => 10;

    public override int Remaining { get; set; } = Duration;

    public override int Initial => Duration;
  }
}
