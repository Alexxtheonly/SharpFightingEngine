using System.Collections.Generic;
using SharpFightingEngine.Battlefields;

namespace SharpFightingEngine.Fighters.Algorithms.TargetFinders
{
  public interface ITargetFinder
  {
    IFighterStats GetTarget(IEnumerable<IFighterStats> fighters, IPosition currentPosition);
  }
}
