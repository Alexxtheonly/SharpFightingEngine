using System;
using System.Collections.Generic;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Combat;
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

      if (!attack.HasEnoughEnergy())
      {
        attackTick.InsufficientEnergy = true;
        return attackTick;
      }

      if ((attack.MissChance(calculationValues) - attack.DodgeChance(calculationValues)).Chance())
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
      return Math.Abs(attack.GetDistance()) <= attack.Skill.Range;
    }

    public static float GetDistance(this IAttack attack)
    {
      return attack.Actor.GetDistance(attack.Target);
    }

    public static int GetDamage(this IAttack attack, EngineCalculationValues calculationValues)
    {
      return (int)(attack.Skill.Damage * ((attack.Actor.Power * calculationValues.AttackPowerFactor) / (attack.Target.Toughness * calculationValues.ArmorDefenseFactor)));
    }

    public static bool HasEnoughEnergy(this IAttack attack)
    {
      return attack.Skill.Energy <= attack.Actor.Energy;
    }

    public static float DodgeChance(this IAttack attack, EngineCalculationValues calculationValues)
    {
      return attack.Target.Agility * calculationValues.AgilityFactor;
    }

    public static float MissChance(this IAttack attack, EngineCalculationValues calculationValues)
    {
      return attack.Actor.Accuracy / calculationValues.AccuracyFactor;
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
