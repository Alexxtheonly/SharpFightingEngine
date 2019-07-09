using System;

namespace SharpFightingEngine.Skills.Melee
{
  public class PunchSkill : ISkill
  {
    public Guid Id => new Guid("F6D2C3E2-908C-4D07-9CB5-598973AE7D4E");

    public string Name => "Punch";

    public int Damage => Energy;

    public float Range => 0.25F;

    public int Energy => 2;
  }
}
