using System;
using System.Collections.Generic;

namespace SharpFightingEngine.Engines
{
  public struct TeamMatchScore : IMatchScore
  {
    public Guid Id { get; set; }

    public int TotalDamageDone { get; set; }

    public int TotalDamageTaken { get; set; }

    public int TotalKills { get; set; }

    public int TotalDeaths { get; set; }

    public float TotalDistanceTraveled { get; set; }

    public int RoundsAlive { get; set; }

    public int TotalHealingDone { get; set; }

    public int TotalHealingRecieved { get; set; }

    public int TotalAssists { get; set; }

    public IEnumerable<Guid> Kills { get; set; }

    public IEnumerable<Guid> Assists { get; set; }
  }
}
