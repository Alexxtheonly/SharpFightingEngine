using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Fighters.Algorithms.SkillFinders
{
  public class DefaultSkillFinder : ISkillFinder
  {
    public ISkill GetSkill(IFighterStats actor, IFighterStats target, IEnumerable<ISkill> skills, EngineCalculationValues calculationValues)
    {
      return skills
        .Where(o => o.Energy <= actor.EnergyRemaining(calculationValues))
        .Where(o => o.Range >= actor.GetDistanceAbs(target))
        .OrderByDescending(o => o.Damage)
        .FirstOrDefault();
    }

    public ISkill GetMaxDamageSkill(IFighterStats actor, IEnumerable<ISkill> skills, EngineCalculationValues calculationValues)
    {
      return skills
        .Where(o => o.Energy <= actor.EnergyRemaining(calculationValues))
        .OrderByDescending(o => o.Damage)
        .FirstOrDefault();
    }

    public ISkill GetMaxRangeSkill(IFighterStats actor, IEnumerable<ISkill> skills, EngineCalculationValues calculationValues)
    {
      return skills
        .Where(o => o.Energy <= actor.EnergyRemaining(calculationValues))
        .OrderByDescending(o => o.Range)
        .FirstOrDefault();
    }

    public IEnumerable<ISkill> ExcludeSkillsOnCooldown(IFighterStats actor, IEnumerable<ISkill> skills, IEnumerable<EngineRoundTick> roundTicks)
    {
      return skills
        .Where(o => o.Cooldown == 0 || !IsOnCooldown(actor, roundTicks, o));
    }

    private static bool IsOnCooldown(IFighterStats actor, IEnumerable<EngineRoundTick> roundTicks, ISkill o)
    {
      return roundTicks
        .GetLastRounds(o.Cooldown)
        .OfType<FighterAttackTick>()
        .Any(t => t.Fighter.Id == actor.Id && t.Skill.Id == o.Id);
    }
  }
}
