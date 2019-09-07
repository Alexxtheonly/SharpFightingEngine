using System;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Range
{
  public class StoneThrowSkill : SkillBase
  {
    public override Guid Id => new Guid("A99A67A3-F03A-4D5B-8DBD-F34DD886FA78");

    public override string Name => "Stone Throw";

    public override float Range => 2.6F;

    public override int Energy => 2;

    public override int DamageLow => 1;

    public override int DamageHigh => 4;
  }
}
