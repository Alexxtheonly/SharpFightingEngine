using System;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Melee
{
  public class SmashSkill : SkillBase
  {
    public override Guid Id => new Guid("7B71CEE4-0567-4786-986E-B30BA2E83E17");

    public override string Name => "Smash";

    public override float Range => 1;

    public override int Energy => 8;

    public override int DamageLow => 6;

    public override int DamageHigh => 10;
  }
}
