using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Fighters
{
  public static class IFighterExtension
  {
    /// <summary>
    /// Returns all other fighters within sight.
    /// </summary>
    public static IEnumerable<IFighterStats> GetVisibleFightersFor(this IEnumerable<IFighterStats> fighters, IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighters.Where(o => o.Id != fighter.Id && o.GetDistance(fighter) <= fighter.VisualRange(calculationValues));
    }

    /// <summary>
    /// Indicates if the fighter is alive.
    /// </summary>
    public static bool IsAlive(this IFighterStats fighter)
    {
      return fighter.Health > 0;
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
      return (int)((fighter.Vitality * calculationValues.VitalityFactor) - fighter.DamageTaken);
    }

    /// <summary>
    /// The Fighter's Energy
    /// </summary>
    public static int Energy(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return (int)((fighter.Stamina * calculationValues.StaminaFactor) - fighter.EnergyUsed);
    }

    /// <summary>
    /// The fighter's range of sight
    /// </summary>
    public static float VisualRange(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.Vision * calculationValues.VisualRangeFactor;
    }

    public static float CriticalHitChance(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      return fighter.Expertise * calculationValues.CriticalHitChanceFactor;
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

      var recoveredDamage = (int)(fighter.Regeneration * calculationValues.HealthRegenerationFactor);
      recoveredDamage = Math.Min(fighter.DamageTaken, recoveredDamage);

      fighter.DamageTaken -= recoveredDamage;

      return new FighterRegenerateHealthTick()
      {
        Fighter = fighter.AsStruct(),
        HealthPointsRegenerated = recoveredDamage,
      };
    }

    public static FighterRegenerateEnergyTick RegenerateEnergy(this IFighterStats fighter, EngineCalculationValues calculationValues)
    {
      if (fighter.EnergyUsed == 0)
      {
        return null;
      }

      var regeneratedEnergy = (int)(fighter.Stamina * calculationValues.EnergyRegenerationFactor);
      regeneratedEnergy = Math.Min(fighter.EnergyUsed, regeneratedEnergy);

      fighter.EnergyUsed -= regeneratedEnergy;

      return new FighterRegenerateEnergyTick()
      {
        Fighter = fighter.AsStruct(),
        RegeneratedEnergy = regeneratedEnergy,
      };
    }

    public static float OffensivePowerLevel(this IFighterStats fighter)
    {
      return CalculatePowerLevel(fighter, typeof(IOffensiveStats).GetProperties());
    }

    public static float DefensivePowerLevel(this IFighterStats fighter)
    {
      return CalculatePowerLevel(fighter, typeof(IDefensiveStats).GetProperties());
    }

    public static float UtilityPowerLevel(this IFighterStats fighter)
    {
      return CalculatePowerLevel(fighter, typeof(IUtilityStats).GetProperties());
    }

    public static float PowerLevel(this IFighterStats fighter)
    {
      return fighter.OffensivePowerLevel() + fighter.DefensivePowerLevel() + fighter.UtilityPowerLevel();
    }

    private static float CalculatePowerLevel(IFighterStats fighter, IEnumerable<PropertyInfo> properties)
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
