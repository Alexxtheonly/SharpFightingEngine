using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Combat
{
  public static class IHealExtension
  {
    public static IEnumerable<EngineTick> Handle(
      this IHeal heal,
      Dictionary<Guid, IFighterStats> fighters,
      IEnumerable<EngineRoundTick> roundTicks,
      EngineCalculationValues calculationValues)
    {
      var ticks = new List<EngineTick>();

      var healTick = heal.GetFighterHealTick();
      ticks.Add(healTick);

      if (heal.IsOnCooldown(roundTicks))
      {
        healTick.OnCooldown = true;
        return ticks;
      }

      if (!heal.IsInRange())
      {
        healTick.OutOfRange = true;
        return ticks;
      }

      healTick.PotentialHealing = heal.GetHealValue(calculationValues);

      var target = fighters[heal.Target.Id];
      healTick.AppliedHealing = target.Heal(healTick.PotentialHealing);
      target.DamageTaken -= healTick.AppliedHealing;

      var additionalTicks = heal.Skill.Perform(heal.Actor, heal.Target, calculationValues);
      foreach (var tick in additionalTicks.OfType<FighterTick>())
      {
        tick.Handle(fighters);
      }

      ticks.AddRange(additionalTicks);

      return ticks;
    }

    public static FighterHealTick GetFighterHealTick(this IHeal heal)
    {
      return new FighterHealTick()
      {
        Fighter = heal.Actor.AsStruct(),
        Target = heal.Target.AsStruct(),
        HealSkill = heal.Skill,
      };
    }

    public static bool IsOnCooldown(this IHeal heal, IEnumerable<EngineRoundTick> roundTicks)
    {
      return heal.Skill.Cooldown > 0 && roundTicks
        .GetLastRounds(heal.Skill.Cooldown)
        .OfType<FighterHealTick>()
        .Where(o => o.Fighter.Id == heal.Actor.Id)
        .Any(o => o.Skill.Id == heal.Skill.Id);
    }

    public static bool IsInRange(this IHeal heal)
    {
      return heal.GetDistance() <= heal.Skill.Range;
    }

    public static float GetDistance(this IHeal heal)
    {
      return heal.Actor.GetDistanceAbs(heal.Target);
    }

    public static int GetHealValue(this IHeal heal, EngineCalculationValues calculationValues)
    {
      return heal.Skill.Heal * (1 + (int)(heal.Actor.GetAdjustedStats().HealingPower * calculationValues.HealingPowerFactor));
    }
  }
}
