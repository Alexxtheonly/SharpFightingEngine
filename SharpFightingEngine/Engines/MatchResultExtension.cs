using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Engines
{
  public static class MatchResultExtension
  {
    public static IEnumerable<MatchScore> CreateMatchScores<T>(this IEnumerable<IGrouping<Guid, T>> groupedScoreTicks)
      where T : IEngineRoundScoreTick
    {
      return groupedScoreTicks
        .Select(o => new MatchScore()
        {
          Id = o.Key,
          Powerlevel = o.Max(u => u.Powerlevel),
          MaxHealth = o.Max(u => u.Health),
          MaxEnergy = o.Max(u => u.Energy),
          TotalDamageDone = o.Sum(u => u.DamageDone),
          TotalDamageTaken = o.Sum(u => u.DamageTaken),
          TotalEnergyUsed = o.Sum(u => u.EnergyUsed),
          TotalKills = o.Sum(u => u.Kills),
          TotalDeaths = o.Sum(u => u.Deaths),
          TotalDistanceTraveled = o.Sum(u => u.DistanceTraveled),
          TotalRegeneratedHealth = o.Sum(u => u.RestoredHealth),
          TotalRegeneratedEnergy = o.Sum(u => u.RestoredEnergy),
          RoundsAlive = o.Max(u => u.Round),
        });
    }

    public static IEnumerable<MatchScore> OrderScores(this IEnumerable<MatchScore> scores)
    {
      return scores
        .OrderBy(o => o.TotalDeaths)
        .ThenByDescending(o => o.TotalKills)
        .ThenByDescending(o => o.RoundsAlive)
        .ThenByDescending(o => o.TotalDamageDone);
    }
  }
}
