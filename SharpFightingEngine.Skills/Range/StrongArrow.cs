using System;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Range
{
  public class StrongArrow : SkillBase
  {
    public override Guid Id => new Guid("E180B5CD-69D8-41F0-8004-BAC2E03CA134");

    public override string Name => "Strong Arrow";

    public override int DamageLow => 8;

    public override int DamageHigh => 18;

    public override float Range => 12;

    public override int Energy => 5;

    public override int Cooldown => 0;
  }
}
