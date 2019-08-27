using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Combat;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Fighters
{
  public class AdvancedFighter : FighterBase
  {
    private int? maxHealth;
    private bool flight;
    private IPosition flightPosition;

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

      SetMaxHealth(roundTicks);

      var currentAttackers = GetCurrentAttackers(roundTicks, visibleEnemies);
      var attackerCount = currentAttackers.Count();

      var lastTargets = GetCurrentTargetChallenges(roundTicks)
        .Where(o => visibleEnemies.Any(e => e.Id == o.Id));

      var healthPercent = Health / maxHealth;

      if (!flight && (healthPercent < 0.33 && attackerCount > 1))
      {
        flight = true;
        flightPosition = PathFinder.GetEscapePath(this, currentAttackers.Select(o => o.Fighter).ToList(), battlefield);
      }

      if (flight && attackerCount > 1 && !this.IsEqualPosition(flightPosition))
      {
        return new Move()
        {
          Actor = this,
          NextPosition = flightPosition,
        };
      }
      else
      {
        flight = false;
      }

      IFighterStats target = null;
      if (currentAttackers.Any())
      {
        target = currentAttackers.First().Fighter;
      }
      else if (lastTargets.Any())
      {
        var targetChallenge = lastTargets
          .OrderByDescending(o => o.Challenge)
          .First();

        target = visibleEnemies.FirstOrDefault(o => o.Id == targetChallenge.Id);
      }

      if (target == null)
      {
        target = TargetFinder.GetTarget(visibleEnemies, this);
      }

      var skill = SkillFinder.GetSkill(this, target, Skills);
      if (skill == null)
      {
        return GetSkillMove(battlefield, target);
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
      var currentRound = roundTicks.GetMaxRound();

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
