using System;

namespace SharpFightingEngine.Fighters
{
  internal struct Fighter : IFighter
  {
    public Guid Id { get; set; }

    public Guid? Team { get; set; }

    public int Health { get; set; }

    public int Energy { get; set; }

    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }

    public int DamageTaken { get; set; }

    public int EnergyUsed { get; set; }
  }
}
