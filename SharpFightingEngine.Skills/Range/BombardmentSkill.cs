using System;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Range
{
  public class BombardmentSkill : SkillBase
  {
    public override Guid Id => new Guid("3F4FE7FC-497E-4894-B5B3-D9102C65728B");

    public override string Name => "Bombardment";

    public override float Range => 9;

    public override int Energy => 15;

    public override int DamageLow => 10;

    public override int DamageHigh => 20;
  }
}
