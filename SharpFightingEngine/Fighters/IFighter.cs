using System;
using System.Collections.Generic;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Skills;

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

    ICollection<IExpiringState> States { get; set; }
  }
}
