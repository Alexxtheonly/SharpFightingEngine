using System;

namespace SharpFightingEngine.Skills.Melee
{
  public class FrenzySmashSkill : ISkill
  {
    public Guid Id => new Guid("A3480692-AF37-4D23-95DA-50C005F82929");

    public string Name => "Frenzy Smash";

    public int Damage => Energy;

    public float Range => 0.25F;

    public int Energy => 10;
  }
}
