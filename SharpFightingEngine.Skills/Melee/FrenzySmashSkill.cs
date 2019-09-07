using System;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Melee
{
  public class FrenzySmashSkill : SkillBase
  {
    public override Guid Id => new Guid("A3480692-AF37-4D23-95DA-50C005F82929");

    public override string Name => "Frenzy Smash";

    public override float Range => 1;

    public override int Energy => 10;

    public override int DamageLow => 7;

    public override int DamageHigh => 13;
  }
}
