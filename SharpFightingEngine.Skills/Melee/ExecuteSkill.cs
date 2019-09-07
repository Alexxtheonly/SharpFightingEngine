using System;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Melee
{
  public class ExecuteSkill : SkillBase
  {
    public override Guid Id => new Guid("8C8229C8-A0A8-47F0-AD04-3DBE61EC6D32");

    public override string Name => "Execute";

    public override float Range => 1;

    public override int Energy => 20;

    public override int DamageLow => 14;

    public override int DamageHigh => 28;
  }
}
