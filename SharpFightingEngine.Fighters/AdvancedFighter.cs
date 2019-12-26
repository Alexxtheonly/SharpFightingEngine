using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Combat;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters.Intelligence;

namespace SharpFightingEngine.Fighters
{
  public class AdvancedFighter : FighterBase
  {
    private int? maxHealth;
    private bool flight;
    private IPosition flightPosition;

    private IntelligenceProvider intelligenceProvider = new IntelligenceProvider();

    public override IFighterAction GetFighterAction(
      IEnumerable<IFighterStats> visibleFighters,
      IBattlefield battlefield,
      IEnumerable<EngineRoundTick> roundTicks,
      EngineCalculationValues calculationValues)
    {
      var visibleEnemies = visibleFighters
        .Where(o => o.Team == null || o.Team != Team);

      if (!visibleEnemies.Any())
      {
        return new Move()
        {
          Actor = this,
          NextPosition = PathFinder.GetRoamingPath(this, battlefield),
        };
      }

      SetMaxHealth(roundTicks);

      var healthPercent = this.HealthRemaining(calculationValues) / (double)maxHealth;

      if (healthPercent <= 0.4)
      {
        var healskill = SkillFinder.GetHealSkill(this, Skills, roundTicks, calculationValues);
        if (healskill != null)
        {
          return new Heal()
          {
            Actor = this,
            Target = this,
            Skill = healskill,
          };
        }
      }

      var intels = intelligenceProvider.GetIntels(this, roundTicks, calculationValues, visibleEnemies);

      var attackers = visibleEnemies.Where(o => intels.Where(i => i.LastTarget == Id).Select(i => i.Id).Contains(o.Id));
      if (attackers.Count() >= 2)
      {
        return new Move()
        {
          Actor = this,
          NextPosition = PathFinder.GetEscapePath(this, attackers, battlefield),
        };
      }

      var bestTarget = intels
        .OrderByDescending(o => o.LastTarget == Id)
        .ThenByDescending(o => o.IsInRange)
        .ThenByDescending(o => o.IsStunned)
        .ThenByDescending(o => o.IsHealingReduced)
        .ThenBy(o => o.HealthPercent)
        .ThenByDescending(o => o.LastRoundHealSkillUsed)
        .ThenBy(o => o.OtherFightersNearby)
        .FirstOrDefault();

      IFighterStats target = visibleEnemies.First(o => o.Id == bestTarget.Id);

      var skill = SkillFinder.GetSkill(this, target, SkillFinder.ExcludeSkillsOnCooldown(this, Skills, roundTicks), calculationValues);
      if (skill == null)
      {
        return GetSkillMove(battlefield, target, roundTicks, calculationValues);
      }

      return new Attack()
      {
        Actor = this,
        Skill = skill,
        Target = target,
      };
    }

    private void SetMaxHealth(IEnumerable<EngineRoundTick> roundTicks)
    {
      if (maxHealth == null)
      {
        var spawntick = roundTicks
          .Where(o => o.Round == 0)
          .SelectMany(o => o.Ticks)
          .OfType<FighterSpawnTick>()
          .Where(o => o.Fighter.Id == Id)
          .FirstOrDefault();

        maxHealth = spawntick?.Fighter.Health ?? throw new Exception("could not find spawn tick");
      }
    }

    /// <summary>
    /// Returns all current attackers. Current means this and last round.
    /// </summary>
    /// <param name="roundTicks"></param>
    /// <returns></returns>
    private IEnumerable<AttackerThreat> GetCurrentAttackers(IEnumerable<EngineRoundTick> roundTicks, IEnumerable<IFighterStats> enemies)
    {
      var currentRound = roundTicks.GetLastRound();

      return roundTicks
        .GetRounds(currentRound.Round - 1, currentRound.Round)
        .OfType<FighterAttackTick>()
        .Where(o => o.Target.Id == Id)
        .GroupBy(o => o.Fighter.Id)
        .Select(o => new AttackerThreat()
        {
          Fighter = enemies.FirstOrDefault(f => f.Id == o.Key),
          Threat = o.Sum(a => a.Damage),
        })
        .Where(o => o.Fighter != null)
        .OrderByDescending(o => o.Threat)
        .ToList();
    }

    /// <summary>
    /// Returns the current targets including the challenge of killing them.
    /// </summary>
    /// <param name="roundTicks"></param>
    /// <returns></returns>
    private IEnumerable<TargetChallenge> GetCurrentTargetChallenges(IEnumerable<EngineRoundTick> roundTicks)
    {
      var currentRound = roundTicks.GetLastRound();

      return roundTicks
        .GetRounds(currentRound.Round - 1, currentRound.Round)
        .OfType<FighterAttackTick>()
        .Where(o => o.Fighter.Id == Id)
        .GroupBy(o => o.Target)
        .Select(o => new TargetChallenge()
        {
          Id = o.Key.Id,
          Challenge = o.Average(a => a.Damage) / o.Key.Health,
        });
    }

    private class AttackerThreat
    {
      public IFighterStats Fighter { get; set; }

      /// <summary>
      /// The higher the value, the higher the threat posed by this attacker.
      /// </summary>
      public double Threat { get; set; }
    }

    private class TargetChallenge
    {
      public Guid Id { get; set; }

      public double Challenge { get; set; }
    }
  }
}
