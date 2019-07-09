using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Fighters.Algorithms.SkillFinders
{
  public class DefaultSkillFinder : ISkillFinder
  {
    public ISkill GetSkill(IFighterStats actor, IFighterStats target, IEnumerable<ISkill> skills)
    {
      return skills
        .Where(o => o.Energy <= actor.Energy)
        .Where(o => o.Range >= actor.GetDistance(target))
        .OrderByDescending(o => o.Damage)
        .FirstOrDefault();
    }
  }
}
