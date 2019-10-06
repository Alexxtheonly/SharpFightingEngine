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
        var attacks = group.OfType<FighterAttackTick>().Where(o => o.Fighter.Id == group.Key);

        int kills = 0;
        foreach (var mutualKill in deaths.Where(o => o.Fighter.Id != group.Key))
        {
          var lastSpawn = roundTicks
            .SelectMany(o => o.Ticks)
            .OfType<FighterSpawnTick>()
            .Where(o => o.DateTime <= mutualKill.DateTime)
            .OrderByDescending(o => o.DateTime)
            .First();

          var attacksOnDeadFighter = attacks
            .Where(o => o.Hit && o.Target.Id == mutualKill.Fighter.Id)
            .Where(o => o.DateTime <= mutualKill.DateTime && o.DateTime >= lastSpawn.DateTime);

          var conditionTicksOnDeadFigher = roundTicks
            .SelectMany(o => o.Ticks)
            .OfType<FighterConditionDamageTick>()
            .Where(o => o.Source.Id == group.Key)
            .Where(o => o.DateTime <= mutualKill.DateTime && o.DateTime >= lastSpawn.DateTime);

          int totalConditionDamage = conditionTicksOnDeadFigher.Sum(o => o.Damage);
          int totalSkillDamage = attacksOnDeadFighter.Sum(o => o.Damage);
          int totalDamage = totalSkillDamage + totalConditionDamage;

          double totalHealth = lastSpawn.Fighter.Health;
          if (totalDamage / totalHealth >= PercentOfTotalHealthNeeded)
          {
            kills++;
          }
        }

        // maybe you ask yourself in some time, why you did not just count the damage above => if a fighter doesn't die the inflicted damage is not evaluated
        var totalSkillDamageDone = roundTicks
          .SelectMany(o => o.Ticks)
          .OfType<FighterAttackTick>()
          .Where(o => o.Fighter.Id == group.Key)
          .Where(o => o.Hit)
          .Sum(o => o.Damage);

        var totalConditionDamageDone = roundTicks
          .SelectMany(o => o.Ticks)
          .OfType<FighterConditionDamageTick>()
          .Where(o => o.Source.Id == group.Key)
          .Sum(o => o.Damage);

        var totalDamageDone = totalSkillDamageDone + totalConditionDamageDone;

        var spawn = spawns.FirstOrDefault(o => o.Fighter.Id == group.Key);
        yield return new FighterMatchScore()
        {
          Id = group.Key,
          TeamId = spawn.Fighter.Team,
          MaxHealth = spawn.Fighter.Health,
          RoundsAlive = roundTicks.Where(o => o.Ticks.OfType<FighterTick>().Any(t => t.Fighter.Id == group.Key)).GetLastRound().Round,
          TotalDamageDone = totalDamageDone,
          TotalDamageTaken = roundTicks.SelectMany(o => o.Ticks).OfType<FighterAttackTick>().Where(o => o.Target.Id == group.Key && o.Hit).Sum(o => o.Damage),
          TotalDeaths = group.OfType<EngineFighterDiedTick>().Where(o => o.Fighter.Id == group.Key).Count(), // todo: o.Fighter.Id == group.Key necessary?
          TotalDistanceTraveled = group.OfType<FighterMoveTick>().Where(o => o.Fighter.Id == group.Key).Sum(o => o.Current.GetDistance(o.Next)), // todo: o.Fighter.Id == group.Key necessary?
          TotalKills = kills,
          TotalHealingDone = group.OfType<FighterHealTick>().Sum(o => o.AppliedHealing),
          TotalHealingRecieved = 0, // todo
        };
      }
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
  }
}
