using System;

namespace SharpFightingEngine.Skills.Range
{
  public class BombardmentSkill : ISkill
  {
    public Guid Id => new Guid("3F4FE7FC-497E-4894-B5B3-D9102C65728B");

    public string Name => "Bombardment";

    public int Damage => Energy;

    public float Range => 9;

    public int Energy => 15;
  }
}
