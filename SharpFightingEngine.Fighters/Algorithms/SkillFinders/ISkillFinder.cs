using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Fighters.Algorithms.SkillFinders
{
  public interface ISkillFinder
  {
    IDamageSkill GetSkill(IFighterStats actor, IFighterStats target, IEnumerable<ISkill> skills, EngineCalculationValues calculationValues);

    IDamageSkill GetMaxRangeSkill(IFighterStats actor, IEnumerable<ISkill> skills, EngineCalculationValues calculationValues);

    IDamageSkill GetMaxDamageSkill(IFighterStats actor, IEnumerable<ISkill> skills, EngineCalculationValues calculationValues);

    IHealSkill GetHealSkill(IFighterStats actor, IEnumerable<ISkill> skills, IEnumerable<EngineRoundTick> roundTicks, EngineCalculationValues calculationValues);

    IEnumerable<ISkill> ExcludeSkillsOnCooldown(IFighterStats actor, IEnumerable<ISkill> skills, IEnumerable<EngineRoundTick> roundTicks);
  }
}
