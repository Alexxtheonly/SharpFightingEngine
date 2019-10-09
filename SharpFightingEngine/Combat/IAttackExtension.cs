using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills.Buffs;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Combat
{
  public static class IAttackExtension
  {
    public static IEnumerable<EngineTick> Handle(
      this IAttack attack,
      Dictionary<Guid, IFighterStats> fighters,
      IEnumerable<EngineRoundTick> roundTicks,
      EngineCalculationValues calculationValues)
    {
      var ticks = new List<EngineTick>();

      var attackTick = attack.GetFighterTick<FighterAttackTick>();

      ticks.Add(attackTick);

      if (attack.IsOnCooldown(roundTicks))
      {
        attackTick.OnCooldown = true;
        return ticks;
      }

      if (!attack.IsInRange())
      {
        attackTick.OutOfRange = true;
        return ticks;
      }

      if (!attack.HitValue(calculationValues).Chance())
      {
        attackTick.Dodged = true;
        return ticks;
      }

      if (attack.Skill.CanBeReflected && (attack.Target.States.OfType<ISkillBuff>().Max(o => o.ReflectChance) ?? 0).Chance())
      {
        attackTick.Reflected = true;
        attackTick.Target = attack.Actor.AsStruct();
        attack.Target = attack.Actor;
      }

      attackTick.Damage = attack.GetDamage(calculationValues);
      if (attack.Actor.CriticalHitChance(calculationValues).Chance())
      {
        attackTick.Damage = (int)(attackTick.Damage * attack.Actor.PotentialFerocity(calculationValues));
        attackTick.Critical = true;
      }

      fighters[attack.Target.Id].DamageTaken += attackTick.Damage;

      var additionalTicks = attack.Skill.Perform(attack.Actor, attack.Target, calculationValues);
      foreach (var tick in additionalTicks.OfType<FighterTick>())
      {
        tick.Handle(fighters);
      }

      ticks.AddRange(additionalTicks);

      return ticks;
    }

    public static bool IsInRange(this IAttack attack)
    {
      return attack.GetDistance() <= attack.Skill.Range;
    }

    public static bool IsOnCooldown(this IAttack attack, IEnumerable<EngineRoundTick> roundTicks)
    {
      return attack.Skill.Cooldown > 0 && roundTicks
        .GetLastRounds(attack.Skill.Cooldown)
        .OfType<FighterAttackTick>()
        .Where(o => o.Fighter.Id == attack.Actor.Id)
        .Any(o => o.AttackSkill.Id == attack.Skill.Id);
    }

    public static float GetDistance(this IAttack attack)
    {
      return attack.Actor.GetDistanceAbs(attack.Target);
    }

    public static int GetDamage(this IAttack attack, EngineCalculationValues calculationValues)
    {
      // (Weapon strength * Power * Skill coefficient) / Armor
      return (int)((attack.Skill.Damage * attack.Actor.GetAdjustedStats().Level * attack.Actor.PotentialPower(calculationValues)) / attack.Target.PotentialDefense(calculationValues));
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

    public static void Handle(this FighterTick tick, Dictionary<Guid, IFighterStats> fighters)
    {
      if (tick.GetType() == typeof(FighterMovedByAttackTick))
      {
        ((FighterMovedByAttackTick)tick).Handle(fighters);
      }
    }

    public static void Handle(this FighterMovedByAttackTick tick, Dictionary<Guid, IFighterStats> fighters)
    {
      fighters[tick.Target.Id].Set(tick.Next);
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
