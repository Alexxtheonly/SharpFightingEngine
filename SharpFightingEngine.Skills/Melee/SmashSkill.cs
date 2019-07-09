using System;

namespace SharpFightingEngine.Skills.Melee
{
  public class SmashSkill : ISkill
  {
    public Guid Id => new Guid("7B71CEE4-0567-4786-986E-B30BA2E83E17");

    public string Name => "Smash";

    public int Damage => Energy;

    public float Range => 0.4F;

    public int Energy => 8;
  }
}
