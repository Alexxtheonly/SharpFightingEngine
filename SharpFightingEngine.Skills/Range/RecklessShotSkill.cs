using System;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Range
{
  public class RecklessShotSkill : SkillBase
  {
    public override Guid Id => new Guid("69D6E8C5-3FAE-4602-A66C-236A5C4271C7");

    public override string Name => "Reckless Shot";

    public override float Range => 3.5F;

    public override int Energy => 9;

    public override int DamageLow => 6;

    public override int DamageHigh => 14;
  }
}
