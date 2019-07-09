using System;

namespace SharpFightingEngine.Skills.Range
{
  public class StoneThrowSkill : ISkill
  {
    public Guid Id => new Guid("A99A67A3-F03A-4D5B-8DBD-F34DD886FA78");

    public string Name => "Stone Throw";

    public int Damage => Energy;

    public float Range => 2.6F;

    public int Energy => 2;
  }
}
