using System;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Range
{
  public class PerforateSkill : SkillBase
  {
    public override Guid Id => new Guid("ABE67626-471D-499D-AA44-2C1468771C3E");

    public override string Name => "Perforate";

    public override float Range => 6;

    public override int Energy => 7;

    public override int DamageLow => 5;

    public override int DamageHigh => 9;
  }
}
