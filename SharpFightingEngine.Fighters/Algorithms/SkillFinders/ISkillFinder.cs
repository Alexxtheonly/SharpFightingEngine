using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Fighters.Algorithms.SkillFinders
{
  public interface ISkillFinder
  {
    ISkill GetSkill(IFighterStats actor, IFighterStats target, IEnumerable<ISkill> skills, EngineCalculationValues calculationValues);

    ISkill GetMaxRangeSkill(IFighterStats actor, IEnumerable<ISkill> skills, EngineCalculationValues calculationValues);

    ISkill GetMaxDamageSkill(IFighterStats actor, IEnumerable<ISkill> skills, EngineCalculationValues calculationValues);
  }
}
