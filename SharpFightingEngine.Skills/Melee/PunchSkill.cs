using System;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Melee
{
  public class PunchSkill : SkillBase
  {
    public override Guid Id => new Guid("F6D2C3E2-908C-4D07-9CB5-598973AE7D4E");

    public override string Name => "Punch";

    public override float Range => 1;

    public override int Energy => 2;

    public override int DamageLow => 1;

    public override int DamageHigh => 4;
  }
}
