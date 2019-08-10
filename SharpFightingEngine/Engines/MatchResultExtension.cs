using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Engines
{
  public static class MatchResultExtension
  {
    public static IEnumerable<FighterMatchScore> CreateFighterMatchScores(this IEnumerable<IGrouping<Guid, EngineRoundScoreTick>> groupedScoreTicks)
    {
      return groupedScoreTicks
        .Select(o => SetScores(
          new FighterMatchScore()
          {
            Id = o.Key,
            TeamId = o.Max(u => u.TeamId),
          }, o));
    }

    public static IEnumerable<TeamMatchScore> CreateTeamMatchScores(this IEnumerable<IGrouping<Guid, EngineRoundTeamScoreTick>> groupedScoreTicks)
    {
      return groupedScoreTicks
        .Select(o => SetScores(
          new TeamMatchScore()
          {
            Id = o.Key,
          }, o));
    }

    public static IEnumerable<T> OrderScores<T>(this IEnumerable<T> scores)
      where T : IMatchScore
    {
      return scores
        .OrderBy(o => o.TotalDeaths)
        .ThenByDescending(o => o.TotalKills)
        .ThenByDescending(o => o.RoundsAlive)
        .ThenByDescending(o => o.TotalDamageDone);
    }

    private static T SetScores<T>(T score, IGrouping<Guid, IEngineRoundScoreTick> ticks)
      where T : IMatchScore
    {
      score.Powerlevel = ticks.Max(u => u.Powerlevel);
      score.MaxHealth = ticks.Max(u => u.Health);
      score.MaxEnergy = ticks.Max(u => u.Energy);
      score.TotalDamageDone = ticks.Sum(u => u.DamageDone);
      score.TotalDamageTaken = ticks.Sum(u => u.DamageTaken);
      score.TotalEnergyUsed = ticks.Sum(u => u.EnergyUsed);
      score.TotalKills = ticks.Sum(u => u.Kills);
      score.TotalDeaths = ticks.Sum(u => u.Deaths);
      score.TotalDistanceTraveled = ticks.Sum(u => u.DistanceTraveled);
      score.TotalRegeneratedHealth = ticks.Sum(u => u.RestoredHealth);
      score.TotalRegeneratedEnergy = ticks.Sum(u => u.RestoredEnergy);
      score.RoundsAlive = ticks.Max(u => u.Round);

      return score;
    }
  }
}
