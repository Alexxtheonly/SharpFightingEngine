using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Combat;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters.Algorithms.PathFinders;
using SharpFightingEngine.Fighters.Algorithms.SkillFinders;
using SharpFightingEngine.Fighters.Algorithms.TargetFinders;
using SharpFightingEngine.Skills;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Fighters
{
  public abstract class FighterBase : IFighterStats
  {
    public Guid Id { get; set; }

    public Guid? Team { get; set; }

    /// <summary>
    /// Total damage taken. If this is equal or below zero, the fighter is considered dead.
    /// </summary>
    public int DamageTaken { get; set; }

    /// <summary>
    /// Energy used this round
    /// </summary>
    public int EnergyUsed { get; set; }

    /// <summary>
    /// Current health
    /// </summary>
    public int Health { get; set; }

    /// <summary>
    /// Current energy
    /// </summary>
    public int Energy { get; set; }

    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }

    public IPathFinder PathFinder { get; set; } = new DefaultPathFinder();

    public ITargetFinder TargetFinder { get; set; } = new DefaultTargetFinder();

    public ISkillFinder SkillFinder { get; set; } = new DefaultSkillFinder();

    /// <summary>
    /// A collection of all the skills the fighter can use.
    /// </summary>
    public IEnumerable<ISkill> Skills { get; set; } = new ISkill[]
    {
    };

    public ICollection<IExpiringState> States { get; set; } = new List<IExpiringState>();

    public IStats Stats { get; set; } = default(Stats);

    public IFighterAttunement Attunement { get; set; }

    public IStats GetAdjustedStats()
    {
      var stats = Stats.Clone();

      foreach (var state in States)
      {
        state.Apply(stats);
      }

      return stats;
    }

    public virtual IFighterAction GetFighterAction(
      IEnumerable<IFighterStats> visibleFighters,
      IBattlefield battlefield,
      IEnumerable<EngineRoundTick> roundTicks,
      EngineCalculationValues calculationValues)
    {
      var roam = new Move()
      {
        Actor = this,
        NextPosition = PathFinder.GetRoamingPath(this, battlefield),
      };

      // Better not attack team members
      var visibleEnemies = visibleFighters
        .Where(o => o.Team == null || o.Team != Team);

      if (!visibleEnemies.Any())
      {
        // if this fighter can't see any enemy he roams
        return roam;
      }

      var target = TargetFinder.GetTarget(visibleEnemies, this);
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

    protected IFighterAction GetSkillMove(IBattlefield battlefield, IFighterStats target, IEnumerable<EngineRoundTick> roundTicks, EngineCalculationValues calculationValues)
    {
      var desiredSkill = 50F.Chance() ?
        SkillFinder.GetMaxDamageSkill(this, SkillFinder.ExcludeSkillsOnCooldown(this, Skills, roundTicks), calculationValues) :
        SkillFinder.GetMaxRangeSkill(this, SkillFinder.ExcludeSkillsOnCooldown(this, Skills, roundTicks), calculationValues);

      // reduce skill distance by 0.15F to compensate rounding differences
      var distance = (desiredSkill?.Range ?? 1.5F) - 0.15F;

      return new Move()
      {
        Actor = this,
        NextPosition = PathFinder.GetPathToEnemy(this, target, distance, battlefield),
      };
    }
  }
}
