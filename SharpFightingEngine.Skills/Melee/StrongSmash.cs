using System;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Melee
{
  public class StrongSmash : DamageSkillBase
  {
    public override Guid Id => new Guid("58A990AC-A1E2-4C50-A958-3D84690E96BB");

    public override string Name => "Strong Smash";

    public override int DamageLow => 8;

    public override int DamageHigh => 10;

    public override float Range => 1;

    public override int Cooldown => 0;
  }
}
