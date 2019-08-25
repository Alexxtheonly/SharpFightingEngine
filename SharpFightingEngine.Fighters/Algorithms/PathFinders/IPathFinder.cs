using System.Collections.Generic;
using SharpFightingEngine.Battlefields;

namespace SharpFightingEngine.Fighters.Algorithms.PathFinders
{
  public interface IPathFinder
  {
    /// <summary>
    /// Returns the path to the enemy to get into attack range.
    /// </summary>
    IPosition GetPathToEnemy(IPosition current, IPosition enemy, float keepDistance, IBattlefield battlefield);

    /// <summary>
    /// Returns the path when no enemy is in sight.
    /// </summary>
    IPosition GetRoamingPath(IPosition current, IBattlefield battlefield);

    /// <summary>
    /// Returns the path to a position.
    /// </summary>
    IPosition GetPath(IPosition current, IPosition desired, IBattlefield battlefield);

    IPosition GetEscapePath(IPosition current, IEnumerable<IPosition> escape, IBattlefield battlefield);
  }
}
