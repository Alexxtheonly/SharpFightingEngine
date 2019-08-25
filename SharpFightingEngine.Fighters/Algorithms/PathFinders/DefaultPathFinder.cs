using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SharpFightingEngine.Battlefields;

namespace SharpFightingEngine.Fighters.Algorithms.PathFinders
{
  public class DefaultPathFinder : IPathFinder
  {
    private readonly Random random = new Random();

    public IPosition GetEscapePath(IPosition current, IEnumerable<IPosition> escape, IBattlefield battlefield)
    {
      var flightPositions = escape.Select(o => current.GetDirection(o, 20));
      Vector3 sum = Vector3.Zero;
      foreach (var flightPosition in flightPositions)
      {
        sum += flightPosition;
      }

      Vector3 avg = sum / flightPositions.Count();

      var bestPosition = new Position()
      {
      }.Set(avg);

      return bestPosition;
    }

    public IPosition GetPath(IPosition current, IPosition desired, IBattlefield battlefield)
    {
      return desired;
    }

    public IPosition GetPathToEnemy(IPosition current, IPosition enemy, float keepDistance, IBattlefield battlefield)
    {
      var desired = new Position()
      {
      }.Set(enemy.GetDirection(current, keepDistance));

      return GetPath(current, desired, battlefield);
    }

    public IPosition GetRoamingPath(IPosition current, IBattlefield battlefield)
    {
      var roam = new Position()
      {
        X = random.Next((int)battlefield.CurrentBounds.Low.X, (int)battlefield.CurrentBounds.High.X),
        Y = random.Next((int)battlefield.CurrentBounds.Low.Y, (int)battlefield.CurrentBounds.High.Y),
        Z = random.Next((int)battlefield.CurrentBounds.Low.Z, (int)battlefield.CurrentBounds.High.Z),
      };

      return GetPath(current, roam, battlefield);
    }
  }
}
