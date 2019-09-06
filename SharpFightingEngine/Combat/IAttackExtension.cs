﻿using System;
using System.Collections.Generic;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Combat
{
  public static class IAttackExtension
  {
    public static EngineTick Handle(this IAttack attack, Dictionary<Guid, IFighterStats> fighters, EngineCalculationValues calculationValues)
    {
      var attackTick = attack.GetFighterTick<FighterAttackTick>();

      if (!attack.IsInRange())
      {
        attackTick.OutOfRange = true;
        return attackTick;
      }

      if (!attack.HasEnoughEnergy(calculationValues))
      {
        attackTick.InsufficientEnergy = true;
        return attackTick;
      }

      if (!attack.HitValue(calculationValues).Chance())
      {
        attackTick.Dodged = true;
        return attackTick;
      }

      attackTick.Damage = attack.GetDamage(calculationValues);
      if (attack.Actor.CriticalHitChance(calculationValues).Chance())
      {
        attackTick.Damage = (int)(attackTick.Damage * calculationValues.CriticalHitDamageFactor);
        attackTick.Critical = true;
      }

      fighters[attack.Target.Id].DamageTaken += attackTick.Damage;
      fighters[attack.Actor.Id].EnergyUsed += attack.Skill.Energy;

      return attackTick;
    }

    public static bool IsInRange(this IAttack attack)
    {
      return attack.GetDistance() <= attack.Skill.Range;
    }

    public static float GetDistance(this IAttack attack)
    {
      return attack.Actor.GetDistanceAbs(attack.Target);
    }

    public static int GetDamage(this IAttack attack, EngineCalculationValues calculationValues)
    {
      return (int)(attack.Skill.Damage * (attack.Actor.PotentialPower(calculationValues) / attack.Target.PotentialDefense(calculationValues)));
    }

    public static bool HasEnoughEnergy(this IAttack attack, EngineCalculationValues calculationValues)
    {
      return attack.Skill.Energy <= attack.Actor.EnergyRemaining(calculationValues);
    }

    public static float DodgeChance(this IAttack attack, EngineCalculationValues calculationValues)
    {
      return attack.Target.DodgeChance(calculationValues);
    }

    public static float HitChance(this IAttack attack, EngineCalculationValues calculationValues)
    {
      return attack.Actor.HitChance(calculationValues);
    }

    public static float HitValue(this IAttack attack, EngineCalculationValues calculationValues)
    {
      return attack.HitChance(calculationValues) - attack.DodgeChance(calculationValues);
    }

    private static T GetFighterTick<T>(this IAttack attack)
      where T : FighterAttackTick, new()
    {
      return new T()
      {
        Attack = attack,
        Fighter = attack.Actor.AsStruct(),
        Target = attack.Target.AsStruct(),
      };
    }
  }
}
