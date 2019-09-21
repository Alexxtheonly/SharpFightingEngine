using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Engines
{
  public static class MatchResultExtension
  {
    /// <summary>
    /// At least the specified percentage damage must be dealt to be counted as Kill or Assist.
    /// </summary>
    private const float PercentOfTotalHealthNeeded = 0.40F;

    public static IEnumerable<FighterMatchScore> CalculateFighterMatchScores(this IEnumerable<EngineRoundTick> roundTicks)
    {
      var groupedByFighter = roundTicks
        .SelectMany(o => o.Ticks)
        .OfType<FighterTick>()
        .GroupBy(o => o.Fighter.Id);

      var spawns = roundTicks
        .GetRounds(0, 0)
        .OfType<FighterSpawnTick>()
        .ToList();

      var deaths = roundTicks
        .SelectMany(o => o.Ticks)
        .OfType<EngineFighterDiedTick>()
        .ToList();

      foreach (var group in groupedByFighter)
      {
        var spawn = spawns.FirstOrDefault(o => o.Fighter.Id == group.Key);

        var attacks = group.OfType<FighterAttackTick>().Where(o => o.Fighter.Id == group.Key);

        var attacksOnDeadFighters = attacks.Where(o => o.Hit && deaths.Select(d => d.Fighter.Id).Contains(o.Target.Id));

        int kills = 0;
        foreach (var mutualKill in attacksOnDeadFighters.GroupBy(o => o.Target.Id))
        {
          double totalDamage = mutualKill.Sum(o => o.Damage);
          double totalHealth = spawns.First(o => o.Fighter.Id == mutualKill.Key).Fighter.Health;

          if (totalDamage / totalHealth >= PercentOfTotalHealthNeeded)
          {
            kills++;
          }
        }

        yield return new FighterMatchScore()
        {
          Id = group.Key,
          TeamId = spawn.Fighter.Team,
          MaxHealth = spawn.Fighter.Health,
          MaxEnergy = spawn.Fighter.Energy,
          RoundsAlive = roundTicks.Where(o => o.Ticks.OfType<FighterTick>().Any(t => t.Fighter.Id == group.Key)).GetLastRound().Round,
          TotalDamageDone = attacks.Where(o => o.Hit).Sum(o => o.Damage),
          TotalDamageTaken = roundTicks.SelectMany(o => o.Ticks).OfType<FighterAttackTick>().Where(o => o.Target.Id == group.Key && o.Hit).Sum(o => o.Damage),
          TotalDeaths = group.OfType<EngineFighterDiedTick>().Where(o => o.Fighter.Id == group.Key).Count(),
          TotalDistanceTraveled = group.OfType<FighterMoveTick>().Where(o => o.Fighter.Id == group.Key).Sum(o => o.Current.GetDistance(o.Next)),
          TotalEnergyUsed = attacks.Sum(o => o.Energy),
          TotalKills = kills,
          TotalRegeneratedHealth = group.OfType<FighterRegenerateHealthTick>().Sum(o => o.HealthPointsRegenerated),
          TotalRegeneratedEnergy = group.OfType<FighterRegenerateEnergyTick>().Sum(o => o.RegeneratedEnergy),
        };
      }
    }

    public static IEnumerable<T> OrderScores<T>(this IEnumerable<T> scores)
      where T : IMatchScore
    {
      return scores
        .OrderBy(o => o.TotalDeaths)
        .ThenByDescending(o => o.TotalKills)
        .ThenByDescending(o => o.RoundsAlive);
    }
  }
}
