using System;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Range
{
  public class StrongArrow : DamageSkillBase
  {
    public override Guid Id => new Guid("E180B5CD-69D8-41F0-8004-BAC2E03CA134");

    public override string Name => "Strong Arrow";

    public override int DamageLow => 5;

    public override int DamageHigh => 8;

    public override float Range => 12;

    public override int Cooldown => 0;

    public override bool CanBeReflected => true;
  }
}
