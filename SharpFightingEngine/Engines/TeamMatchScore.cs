using System;

namespace SharpFightingEngine.Engines
{
  public struct TeamMatchScore : IMatchScore
  {
    public Guid Id { get; set; }

    public int TotalDamageDone { get; set; }

    public int TotalDamageTaken { get; set; }

    public int TotalEnergyUsed { get; set; }

    public int TotalKills { get; set; }

    public int TotalDeaths { get; set; }

    public float TotalDistanceTraveled { get; set; }

    public int TotalRegeneratedHealth { get; set; }

    public int TotalRegeneratedEnergy { get; set; }

    public int RoundsAlive { get; set; }
  }
}
