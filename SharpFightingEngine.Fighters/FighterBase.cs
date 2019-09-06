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
using SharpFightingEngine.Skills.Melee;
using SharpFightingEngine.Skills.Range;
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

    /// <summary>
    /// Indicates the distance that can be covered in a round.
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    /// Indicates how much energy the fighter has. Energy is used to perform skills.
    /// </summary>
    public float Stamina { get; set; }

    /// <summary>
    /// Indicates how many life points the fighter has.
    /// </summary>
    public float Vitality { get; set; }

    /// <summary>
    /// Indicates the power of the fighter. Power increases the damage caused by abilities.
    /// </summary>
    public float Power { get; set; }

    /// <summary>
    /// Indicates the mobility of the fighter. Mobility increases the chance of avoiding enemy attacks.
    /// </summary>
    public float Agility { get; set; }

    /// <summary>
    /// Indicates the accuracy of the fighter. Accuracy reduces the chance of missing an opponent with an attack.
    /// </summary>
    public float Accuracy { get; set; }

    /// <summary>
    /// Indicates the armor of the fighter. Armor reduces the damage taken by enemy attacks.
    /// </summary>
    public float Toughness { get; set; }

    /// <summary>
    /// Indicates the fighter's ability to regenerate. Regeneration restores life points every round.
    /// </summary>
    public float Regeneration { get; set; }

    /// <summary>
    /// Indicates the vision of the fighter. The vision affects the distance the enemy can be seen at.
    /// </summary>
    public float Vision { get; set; }

    /// <summary>
    /// Indicates the expertise. Expertise increases chance of critical hits with attacks.
    /// </summary>
    public float Expertise { get; set; }

    public IPathFinder PathFinder { get; set; } = new DefaultPathFinder();

    public ITargetFinder TargetFinder { get; set; } = new DefaultTargetFinder();

    public ISkillFinder SkillFinder { get; set; } = new DefaultSkillFinder();

    /// <summary>
    /// A collection of all the skills the fighter can use.
    /// </summary>
    public IEnumerable<ISkill> Skills { get; set; } = new ISkill[]
    {
      new PunchSkill(),
      new StoneThrowSkill(),
      new ExecuteSkill(),
      new FrenzySmashSkill(),
      new SmashSkill(),
      new BombardmentSkill(),
      new PerforateSkill(),
      new RecklessShotSkill(),
    };

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
      var skill = SkillFinder.GetSkill(this, target, Skills, calculationValues);
      if (skill == null)
      {
        return GetSkillMove(battlefield, target, calculationValues);
      }

      return new Attack()
      {
        Actor = this,
        Skill = skill,
        Target = target,
      };
    }

    protected IFighterAction GetSkillMove(IBattlefield battlefield, IFighterStats target, EngineCalculationValues calculationValues)
    {
      var desiredSkill = 50F.Chance() ? SkillFinder.GetMaxDamageSkill(this, Skills, calculationValues) : SkillFinder.GetMaxRangeSkill(this, Skills, calculationValues);
      var distance = desiredSkill?.Range ?? 1;

      return new Move()
      {
        Actor = this,
        NextPosition = PathFinder.GetPathToEnemy(this, target, distance, battlefield),
      };
    }
  }
}
