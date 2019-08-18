using System.Collections.Generic;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Fighters.Algorithms.SkillFinders
{
  public interface ISkillFinder
  {
    ISkill GetSkill(IFighterStats actor, IFighterStats target, IEnumerable<ISkill> skills);

    ISkill GetMaxRangeSkill(IFighterStats actor, IEnumerable<ISkill> skills);

    ISkill GetMaxDamageSkill(IFighterStats actor, IEnumerable<ISkill> skills);
  }
}
