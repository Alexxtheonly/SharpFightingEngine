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
    public IDamageSkill GetSkill(IFighterStats actor, IFighterStats target, IEnumerable<ISkill> skills, EngineCalculationValues calculationValues)
    {
      return skills
        .OfType<IDamageSkill>()
        .Where(o => o.Range >= actor.GetDistanceAbs(target))
        .OrderByDescending(o => o.Damage)
        .FirstOrDefault();
    }

    public IDamageSkill GetMaxDamageSkill(IFighterStats actor, IEnumerable<ISkill> skills, EngineCalculationValues calculationValues)
    {
      return skills
        .OfType<IDamageSkill>()
        .OrderByDescending(o => o.Damage)
        .FirstOrDefault();
    }

    public IDamageSkill GetMaxRangeSkill(IFighterStats actor, IEnumerable<ISkill> skills, EngineCalculationValues calculationValues)
    {
      return skills
        .OfType<IDamageSkill>()
        .OrderByDescending(o => o.Range)
        .FirstOrDefault();
    }

    public IEnumerable<ISkill> ExcludeSkillsOnCooldown(IFighterStats actor, IEnumerable<ISkill> skills, IEnumerable<EngineRoundTick> roundTicks)
    {
      return skills
        .Where(o => o.Cooldown == 0 || !IsOnCooldown(actor, roundTicks, o));
    }

    public IHealSkill GetHealSkill(IFighterStats actor, IEnumerable<ISkill> skills, IEnumerable<EngineRoundTick> roundTicks, EngineCalculationValues calculationValues)
    {
      return ExcludeSkillsOnCooldown(actor, skills, roundTicks)
        .OfType<IHealSkill>()
        .OrderByDescending(o => o.Heal)
        .FirstOrDefault();
    }

    private static bool IsOnCooldown(IFighterStats actor, IEnumerable<EngineRoundTick> roundTicks, ISkill o)
    {
      return roundTicks
        .GetLastRounds(o.Cooldown)
        .OfType<FighterSkillTick>()
        .Any(t => t.Fighter.Id == actor.Id && t.Skill.Id == o.Id);
    }
  }
}
