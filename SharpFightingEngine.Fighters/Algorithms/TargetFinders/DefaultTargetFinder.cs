using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;

namespace SharpFightingEngine.Fighters.Algorithms.TargetFinders
{
  public class DefaultTargetFinder : ITargetFinder
  {
    public IFighterStats GetTarget(IEnumerable<IFighterStats> fighters, IPosition currentPosition)
    {
      return fighters
        .OrderBy(o => o.GetDistanceAbs(currentPosition))
        .FirstOrDefault();
    }
  }
}
