using System;

namespace SharpFightingEngine.Skills.Melee
{
  public class ExecuteSkill : ISkill
  {
    public Guid Id => new Guid("8C8229C8-A0A8-47F0-AD04-3DBE61EC6D32");

    public string Name => "Execute";

    public int Damage => Energy;

    public float Range => 0.3F;

    public int Energy => 20;
  }
}
