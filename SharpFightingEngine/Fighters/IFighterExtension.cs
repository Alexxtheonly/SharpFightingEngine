using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
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
        Energy = fighter.Energy,
        X = fighter.X,
        Y = fighter.Y,
        Z = fighter.Z,
        DamageTaken = fighter.DamageTaken,
        EnergyUsed = fighter.EnergyUsed,
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

    /// <summary>
    /// The Fighter's Energy
    /// </summary>
    public static int Energy(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return (int)(fighter.GetAdjustedStats().Stamina * calculationValues.StaminaFactor);
    }

    public static float PotentialPower(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.GetAdjustedStats().Power * calculationValues.AttackPowerFactor;
    }

    public static float PotentialDefense(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.GetAdjustedStats().Toughness * calculationValues.ArmorDefenseFactor;
    }

    public static int EnergyRemaining(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.Energy(calculationValues) - fighter.EnergyUsed;
    }

    public static int HealthRemaining(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.Health(calculationValues) - fighter.DamageTaken;
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
      return fighter.GetAdjustedStats().Vision * calculationValues.VisualRangeFactor;
    }

    public static float CriticalHitChance(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.GetAdjustedStats().Expertise * calculationValues.CriticalHitChanceFactor;
    }

    public static float DodgeChance(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.GetAdjustedStats().Agility * calculationValues.AgilityFactor;
    }

    public static float HitChance(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return 100 + (fighter.GetAdjustedStats().Accuracy * calculationValues.AccuracyFactor);
    }

    public static int HealthRegeneration(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      var potentialRegeneration = (int)(fighter.GetAdjustedStats().Regeneration * calculationValues.HealthRegenerationFactor);

      var reducedHealing = fighter.States
        .OfType<ISkillCondition>()
        .Where(o => o.HealingReduced != null)
        .Max(o => o.HealingReduced);

      var actualRegeneration = potentialRegeneration;
      if (reducedHealing != null)
      {
        actualRegeneration -= (int)(potentialRegeneration * reducedHealing);
      }

      return actualRegeneration;
    }

    /// <summary>
    /// Restores health points based on the value of regeneration.
    /// </summary>
    /// <returns>Returns how many health points have been regenerated.</returns>
    public static FighterRegenerateHealthTick RegenerateHealth(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      if (fighter.DamageTaken == 0)
      {
        return null;
      }

      var recoveredDamage = Math.Min(fighter.DamageTaken, fighter.HealthRegeneration(calculationValues));
      fighter.DamageTaken -= recoveredDamage;

      return new FighterRegenerateHealthTick()
      {
        Fighter = fighter.AsStruct(),
        HealthPointsRegenerated = recoveredDamage,
      };
    }

    public static int EnergyRegeneration(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return (int)(fighter.Energy(calculationValues) * calculationValues.EnergyRegenerationFactor);
    }

    public static FighterRegenerateEnergyTick RegenerateEnergy(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      if (fighter.EnergyUsed == 0)
      {
        return null;
      }

      var regeneratedEnergy = Math.Min(fighter.EnergyUsed, fighter.EnergyRegeneration(calculationValues));
      fighter.EnergyUsed -= regeneratedEnergy;

      return new FighterRegenerateEnergyTick()
      {
        Fighter = fighter.AsStruct(),
        RegeneratedEnergy = regeneratedEnergy,
      };
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
        powerLevel += (float)statproperty.GetValue(fighter);
      }

      return powerLevel;
    }
  }
}
