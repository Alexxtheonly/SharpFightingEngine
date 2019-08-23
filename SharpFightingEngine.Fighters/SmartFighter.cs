using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Combat;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Fighters
{
  public class SmartFighter : FighterBase
  {
    public override IFighterAction GetFighterAction(IEnumerable<IFighterStats> visibleFighters, IBattlefield battlefield, IEnumerable<EngineRoundTick> roundTicks)
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

      var currentAttackers = GetCurrentAttackers(roundTicks);
      IFighterStats target;

      var attackerCount = currentAttackers.Count();
      switch (attackerCount)
      {
        case 0:
          target = TargetFinder.GetTarget(visibleEnemies, this);
          break;
        default:
          if (attackerCount > 2)
          {
            // too many, get out asap.
            target = null;
            break;
          }

          target = visibleEnemies.FirstOrDefault(o => o.Id == currentAttackers.First().Id);

          if (target != null && (GetProbabilityOfKillingTargetWithoutDying(roundTicks, target.Id) ?? 2) <= -3)
          {
            // chicken out
            target = null;
          }

          break;
      }

      if (target == null)
      {
        // run away asap
        return new Move()
        {
          Actor = this,
          NextPosition = PathFinder.GetRoamingPath(this, battlefield),
        };
      }

      var skill = SkillFinder.GetSkill(this, target, Skills);
      if (skill == null)
      {
        var desiredSkill = 50F.Chance() ? SkillFinder.GetMaxDamageSkill(this, Skills) : SkillFinder.GetMaxRangeSkill(this, Skills);
        var distance = desiredSkill?.Range ?? 1;

        return new Move()
        {
          Actor = this,
          NextPosition = PathFinder.GetPathToEnemy(this, target, distance, battlefield),
        };
      }

      return new Attack()
      {
        Actor = this,
        Skill = skill,
        Target = target,
      };
    }

    /// <summary>
    /// Returns all current attackers. Current means this and last round.
    /// </summary>
    /// <param name="roundTicks"></param>
    /// <returns></returns>
    private IEnumerable<AttackerThreat> GetCurrentAttackers(IEnumerable<EngineRoundTick> roundTicks)
    {
      var currentRound = roundTicks.GetMaxRound();

      return roundTicks
        .GetRounds(currentRound.Round - 1, currentRound.Round)
        .OfType<FighterAttackTick>()
        .Where(o => o.Target.Id == Id)
        .GroupBy(o => o.Fighter.Id)
        .Select(o => new AttackerThreat()
        {
          Id = o.Key,
          Threat = o.Sum(a => a.Damage),
        })
        .OrderByDescending(o => o.Threat);
    }

    /// <summary>
    /// Returns the current targets including the challenge of killing them.
    /// </summary>
    /// <param name="roundTicks"></param>
    /// <returns></returns>
    private IEnumerable<TargetChallenge> GetCurrentTargetChallenges(IEnumerable<EngineRoundTick> roundTicks)
    {
      var currentRound = roundTicks.GetMaxRound();

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

    private double? GetProbabilityOfKillingTargetWithoutDying(IEnumerable<EngineRoundTick> roundTick, Guid targetId)
    {
      var incoming = roundTick
        .Select(o => new
        {
          o.Round,
          Ticks = o.Ticks.OfType<FighterAttackTick>().Where(a => a.Fighter.Id == targetId && a.Target.Id == Id)
        })
        .Where(o => o.Ticks.Any());

      var outgoing = roundTick
        .Select(o => new
        {
          o.Round,
          Ticks = o.Ticks.OfType<FighterAttackTick>().Where(a => a.Target.Id == targetId && a.Fighter.Id == Id)
        })
        .Where(o => o.Ticks.Any());

      if (!incoming.SelectMany(o => o.Ticks).Any() || !outgoing.SelectMany(o => o.Ticks).Any())
      {
        // without any data we can not calculate the probability
        return null;
      }

      var ownMaxRegenerationValue = roundTick
        .SelectMany(o => o.Ticks)
        .OfType<FighterRegenerateHealthTick>()
        .Where(o => o.Fighter.Id == Id)
        .OrderByDescending(o => o.HealthPointsRegenerated)
        .FirstOrDefault()?.HealthPointsRegenerated ?? 0;

      var targetMaxRegenerationValue = roundTick
        .SelectMany(o => o.Ticks)
        .OfType<FighterRegenerateHealthTick>()
        .Where(o => o.Fighter.Id == targetId)
        .OrderByDescending(o => o.HealthPointsRegenerated)
        .FirstOrDefault()?.HealthPointsRegenerated ?? 0;

      var averageIncomingDamage = incoming.Average(o => o.Ticks.Sum(a => a.Damage));
      var averageOutgoingDamage = outgoing.Average(o => o.Ticks.Sum(a => a.Damage));

      var targetHealth = outgoing
        .SelectMany(o => o.Ticks)
        .LastOrDefault()?.Target.Health ?? 0;

      if (targetHealth == 0)
      {
        return null;
      }

      var targetRoundsUntilDeath = targetHealth / (averageOutgoingDamage - targetMaxRegenerationValue);
      var ownRoundsUntilDeath = Health / (averageIncomingDamage - ownMaxRegenerationValue);

      return ownRoundsUntilDeath - targetRoundsUntilDeath;
    }

    private class AttackerThreat
    {
      public Guid Id { get; set; }

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
