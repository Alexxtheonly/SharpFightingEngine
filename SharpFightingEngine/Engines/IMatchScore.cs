﻿using System;
using System.Collections.Generic;

namespace SharpFightingEngine.Engines
{
  public interface IMatchScore
  {
    Guid Id { get; set; }

    int TotalDamageDone { get; set; }

    int TotalDamageTaken { get; set; }

    int TotalKills { get; set; }

    int TotalAssists { get; set; }

    int TotalDeaths { get; set; }

    float TotalDistanceTraveled { get; set; }

    int TotalHealingDone { get; set; }

    int TotalHealingRecieved { get; set; }

    int RoundsAlive { get; set; }

    IEnumerable<Guid> Kills { get; set; }

    IEnumerable<Guid> Assists { get; set; }
  }
}
