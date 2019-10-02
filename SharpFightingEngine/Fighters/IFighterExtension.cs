using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Skills.Conditions;

namespace SharpFightingEngine.Fighters
{
  public static class IFighterExtension
  {
    /// <summary>
    /// Returns all other fighters within sight.
    /// </summary>
    public static IEnumerable<IFighterStats> GetVisibleFightersFor(this IEnumerable<IFighterStats> fighters, IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighters
        .Where(o => o.HealthRemaining(calculationValues) > 0)
        .Where(o => o.Id != fighter.Id && o.GetDistance(fighter) <= fighter.VisualRange(calculationValues));
    }

    /// <summary>
    /// Indicates if the fighter is alive.
    /// </summary>
    public static bool IsAlive(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.HealthRemaining(calculationValues) > 0;
    }

    /// <summary>
    /// Converts the fighter into a struct
    /// </summary>
    public static IFighter AsStruct(this IFighterStats fighter)
    {
      var fighterStruct = new Fighter()
      {
        Id = fighter.Id,
        Team = fighter.Team,
        Health = fighter.Health,
        X = fighter.X,
        Y = fighter.Y,
        Z = fighter.Z,
        DamageTaken = fighter.DamageTaken,
      };

      return fighterStruct;
    }

    /// <summary>
    /// Returns the position of the fighter.
    /// </summary>
    public static Position GetPosition(this IFighter fighter)
    {
      return new Position()
      {
        X = fighter.X,
        Y = fighter.Y,
        Z = fighter.Z,
      };
    }

    /// <summary>
    /// The Fighter's health points
    /// </summary>
    public static int Health(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return (int)(fighter.GetAdjustedStats().Vitality * calculationValues.VitalityFactor);
    }

    public static float PotentialPower(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.GetAdjustedStats().Power * calculationValues.AttackPowerFactor;
    }

    public static float PotentialDefense(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.GetAdjustedStats().Armor * calculationValues.ArmorFactor;
    }

    public static float PotentialConditionPower(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.GetAdjustedStats().ConditionPower * calculationValues.ConditionPowerFactor;
    }

    public static float PotentialHealingPower(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.GetAdjustedStats().HealingPower * calculationValues.HealingPowerFactor;
    }

    public static float PotentialFerocity(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return 1 + (fighter.GetAdjustedStats().Ferocity * calculationValues.FerocityFactor);
    }

    public static int HealthRemaining(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.Health(calculationValues) - fighter.DamageTaken;
    }

    public static int Heal(this IFighterStats fighter, int potentialHeal)
    {
      if (fighter.DamageTaken == 0)
      {
        return 0;
      }

      var reducedHealing = fighter.States
        .OfType<ISkillCondition>()
        .Where(o => o.HealingReduced != null)
        .Max(o => o.HealingReduced);

      var actualHeal = potentialHeal;
      if (reducedHealing != null)
      {
        actualHeal -= (int)(potentialHeal * reducedHealing);
      }

      return Math.Min(fighter.DamageTaken, actualHeal);
    }

    public static int GetConditionDamage(this IFighterStats fighter, EngineCalculationValues calculationValues, ISkillCondition condition)
    {
      return (int)(condition.Damage * (fighter.GetAdjustedStats().ConditionPower * calculationValues.ConditionPowerFactor));
    }

    public static float Velocity(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.GetAdjustedStats().Speed * calculationValues.SpeedFactor;
    }

    /// <summary>
    /// The fighter's range of sight
    /// </summary>
    public static float VisualRange(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.GetAdjustedStats().Vision * calculationValues.VisionFactor;
    }

    public static float CriticalHitChance(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.GetAdjustedStats().Precision * calculationValues.PrecisionFactor;
    }

    public static float DodgeChance(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.GetAdjustedStats().Agility * calculationValues.AgilityFactor;
    }

    public static float HitChance(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return 100 + (fighter.GetAdjustedStats().Accuracy * calculationValues.AccuracyFactor);
    }

    public static float OffensivePowerLevel(this IStats fighter)
    {
      return CalculatePowerLevel(fighter, typeof(IOffensiveStats).GetProperties());
    }

    public static float DefensivePowerLevel(this IStats fighter)
    {
      return CalculatePowerLevel(fighter, typeof(IDefensiveStats).GetProperties());
    }

    public static float UtilityPowerLevel(this IStats fighter)
    {
      return CalculatePowerLevel(fighter, typeof(IUtilityStats).GetProperties());
    }

    public static float PowerLevel(this IStats fighter)
    {
      return fighter.OffensivePowerLevel() + fighter.DefensivePowerLevel() + fighter.UtilityPowerLevel();
    }

    private static float CalculatePowerLevel(IStats fighter, IEnumerable<PropertyInfo> properties)
    {
      float powerLevel = 0;

      foreach (var statproperty in properties)
      {
        powerLevel += (int)statproperty.GetValue(fighter);
      }

      return powerLevel;
    }
  }
}
