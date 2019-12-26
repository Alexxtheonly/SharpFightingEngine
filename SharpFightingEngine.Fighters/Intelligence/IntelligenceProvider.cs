using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Skills.Conditions;

namespace SharpFightingEngine.Fighters.Intelligence
{
  public class IntelligenceProvider
  {
    public IEnumerable<EnemyIntel> GetIntels(
      FighterBase fighter,
      IEnumerable<EngineRoundTick> roundTicks,
      EngineCalculationValues calculationValues,
      IEnumerable<IFighterStats> enemies)
    {
      foreach (var enemy in enemies)
      {
        var intel = new EnemyIntel()
        {
          Id = enemy.Id,
          HealthPercent = enemy.HealthRemaining(calculationValues) / enemy.Health(calculationValues),
        };

        intel.DamageDealt = DamageDone(roundTicks, fighter.Id, enemy.Id);
        intel.DamageTaken = DamageDone(roundTicks, enemy.Id, fighter.Id);

        intel.LastTarget = roundTicks
          .GetLastRounds(2)
          .OfType<FighterAttackTick>()
          .Where(o => o.Fighter.Id == fighter.Id)
          .OrderByDescending(o => o.DateTime)
          .FirstOrDefault()
          ?.Target.Id;

        intel.IsInRange = fighter.SkillFinder
          .GetMaxRangeSkill(fighter, fighter.SkillFinder.ExcludeSkillsOnCooldown(fighter, fighter.Skills, roundTicks), calculationValues)?.Range >= fighter.GetDistance(enemy);

        intel.LastRoundHealSkillUsed = roundTicks
          .Where(o => o.Ticks.OfType<FighterHealTick>().Any(t => t.Target.Id == enemy.Id))
          .GetLastRound()
          ?.Round;

        intel.IsStunned = enemy.States.OfType<ISkillCondition>().Any(o => o.PreventsPerformingActions);
        intel.IsHealingReduced = enemy.States.OfType<ISkillCondition>().Any(o => o.HealingReduced != null);

        intel.OtherFightersNearby = enemies
          .Where(o => o.Id != enemy.Id)
          .Where(o => o.GetDistanceAbs(enemy) <= 10)
          .Count();

        intel.LastAttackers = roundTicks
          .GetLastRounds(2)
          .OfType<FighterAttackTick>()
          .Where(o => o.Target.Id == enemy.Id)
          .Select(o => o.Fighter.Id)
          .Distinct();

        yield return intel;
      }
    }

    private int DamageDone(IEnumerable<EngineRoundTick> roundTicks, Guid attackerId, Guid targetId)
    {
      return roundTicks
          .GetLastRounds(3)
          .OfType<FighterAttackTick>()
          .Where(o => o.Fighter.Id == attackerId && o.Target.Id == targetId)
          .Where(o => o.Hit)
          .Sum(o => o.Damage);
    }
  }
}
