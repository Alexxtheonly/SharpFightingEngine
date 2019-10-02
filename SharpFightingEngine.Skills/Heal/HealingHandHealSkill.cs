using System;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Heal
{
  public class HealingHandHealSkill : HealSkillBase
  {
    public override Guid Id => new Guid("90242095-9466-4A71-829A-C1BF968839E6");

    public override string Name => "Healing Hand";

    public override int Heal => 7;

    public override float Range => 10;

    public override int Cooldown => 2;
  }
}
