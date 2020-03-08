using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Engines
{
  public static class MatchResultExtension
  {
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

        List<Guid> kills = new List<Guid>();
        List<Guid> assists = new List<Guid>();
        foreach (var mutualKill in deaths.Where(o => o.Fighter.Id != group.Key))
        {
          var lastSpawn = roundTicks
            .SelectMany(o => o.Ticks)
            .OfType<FighterSpawnTick>()
            .Where(o => o.DateTime <= mutualKill.DateTime)
            .OrderByDescending(o => o.DateTime)
            .First();

          var attacksOnDeadFighter = attacks
            .Where(o => (o.Hit || o.Reflected) && o.Target.Id == mutualKill.Fighter.Id)
            .Where(o => o.DateTime <= mutualKill.DateTime && o.DateTime >= lastSpawn.DateTime);

          var conditionTicksOnDeadFigher = roundTicks
            .SelectMany(o => o.Ticks)
            .OfType<FighterConditionDamageTick>()
            .Where(o => o.Source.Id == group.Key)
            .Where(o => o.DateTime <= mutualKill.DateTime && o.DateTime >= lastSpawn.DateTime);

          int totalConditionDamage = conditionTicksOnDeadFigher.Sum(o => o.Damage);
          int totalSkillDamage = attacksOnDeadFighter.Sum(o => o.Damage);
          int totalDamage = totalSkillDamage + totalConditionDamage;

          var lastConditionTick = roundTicks
            .SelectMany(o => o.Ticks)
            .OfType<FighterConditionDamageTick>()
            .Where(o => o.Fighter.Id == mutualKill.Fighter.Id)
            .Where(o => o.DateTime <= mutualKill.DateTime && o.DateTime >= lastSpawn.DateTime)
            .LastOrDefault();

          var lastAttackTick = roundTicks
            .SelectMany(o => o.Ticks)
            .OfType<FighterAttackTick>()
            .Where(o => o.Damage > 0 && o.Target.Id == mutualKill.Fighter.Id)
            .Where(o => o.DateTime <= mutualKill.DateTime && o.DateTime >= lastSpawn.DateTime)
            .LastOrDefault();

          if (((lastAttackTick?.DateTime.Ticks ?? 0) > (lastConditionTick?.DateTime.Ticks ?? 0)) && (lastAttackTick?.Fighter.Id == group.Key))
          {
            kills.Add(mutualKill.Fighter.Id);
          }
          else if (((lastConditionTick?.DateTime.Ticks ?? 0) > (lastAttackTick?.DateTime.Ticks ?? 0)) && lastConditionTick?.Source.Id == group.Key)
          {
            kills.Add(mutualKill.Fighter.Id);
          }
          else
          {
            assists.Add(mutualKill.Fighter.Id);
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
          TotalDeaths = group.OfType<EngineFighterDiedTick>().Count(),
          TotalDistanceTraveled = group.OfType<FighterMoveTick>().Sum(o => o.Current.GetDistance(o.Next)),
          TotalKills = kills.Count,
          TotalAssists = assists.Count,
          Kills = kills,
          Assists = assists,
          TotalHealingDone = group.OfType<FighterHealTick>().Sum(o => o.AppliedHealing),
          TotalHealingRecieved = 0, // todo
        };
      }
    }

    public static IEnumerable<TeamMatchScore> CalculateTeamMatchScores(this IEnumerable<FighterMatchScore> fighterScores)
    {
      return fighterScores
        .Where(o => o.TeamId != null)
        .GroupBy(o => o.TeamId)
        .Select(o => new TeamMatchScore()
        {
          Id = o.Key ?? default,
          RoundsAlive = o.Max(x => x.RoundsAlive),
          TotalDamageDone = o.Sum(x => x.TotalDamageDone),
          TotalDamageTaken = o.Sum(x => x.TotalDamageTaken),
          TotalDeaths = o.Sum(x => x.TotalDeaths),
          TotalDistanceTraveled = o.Sum(x => x.TotalDistanceTraveled),
          TotalKills = o.Sum(x => x.TotalKills),
          TotalAssists = o.Sum(x => x.TotalAssists),
          TotalHealingDone = o.Sum(x => x.TotalHealingDone),
          TotalHealingRecieved = o.Sum(x => x.TotalHealingRecieved),
          Kills = o.SelectMany(x => x.Kills),
          Assists = o.SelectMany(x => x.Assists),
        });
    }
  }
}
