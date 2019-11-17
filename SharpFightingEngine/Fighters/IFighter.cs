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

    int Health { get; set; }

    ICollection<IExpiringState> States { get; set; }

    IFighterAttunement Attunement { get; set; }
  }
}
