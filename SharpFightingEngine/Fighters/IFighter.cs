using System;
using SharpFightingEngine.Battlefields;

namespace SharpFightingEngine.Fighters
{
  public interface IFighter : IPosition
  {
    Guid Id { get; }

    Guid? Team { get; }

    int DamageTaken { get; set; }

    int EnergyUsed { get; set; }

    int Health { get; set; }

    int Energy { get; set; }
  }
}
