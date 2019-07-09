using System;

namespace SharpFightingEngine.Skills.Range
{
  public class PerforateSkill : ISkill
  {
    public Guid Id => new Guid("ABE67626-471D-499D-AA44-2C1468771C3E");

    public string Name => "Perforate";

    public int Damage => Energy;

    public float Range => 6;

    public int Energy => 7;
  }
}
