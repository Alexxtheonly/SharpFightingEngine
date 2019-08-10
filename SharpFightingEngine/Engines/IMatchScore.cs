using System;

namespace SharpFightingEngine.Engines
{
  public interface IMatchScore
  {
    Guid Id { get; set; }

    int Powerlevel { get; set; }

    int MaxHealth { get; set; }

    int MaxEnergy { get; set; }

    int TotalDamageDone { get; set; }

    int TotalDamageTaken { get; set; }

    int TotalEnergyUsed { get; set; }

    int TotalKills { get; set; }

    int TotalDeaths { get; set; }

    float TotalDistanceTraveled { get; set; }

    int TotalRegeneratedHealth { get; set; }

    int TotalRegeneratedEnergy { get; set; }

    int RoundsAlive { get; set; }
  }
}
