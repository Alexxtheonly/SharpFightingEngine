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
        .Where(o => o.Energy <= actor.EnergyRemaining())
        .Where(o => o.Range >= actor.GetDistance(target))
        .OrderByDescending(o => o.Damage)
        .FirstOrDefault();
    }

    public ISkill GetMaxDamageSkill(IFighterStats actor, IEnumerable<ISkill> skills)
    {
      return skills
        .Where(o => o.Energy <= actor.EnergyRemaining())
        .OrderByDescending(o => o.Damage)
        .FirstOrDefault();
    }

    public ISkill GetMaxRangeSkill(IFighterStats actor, IEnumerable<ISkill> skills)
    {
      return skills
        .Where(o => o.Energy <= actor.EnergyRemaining())
        .OrderByDescending(o => o.Range)
        .FirstOrDefault();
    }
  }
}
