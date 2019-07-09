using System;
using SharpFightingEngine.Battlefields;

namespace SharpFightingEngine.Fighters.Algorithms.PathFinders
{
  public class DefaultPathFinder : IPathFinder
  {
    public IPosition GetPath(IPosition current, IPosition desired, IBattlefield battlefield)
    {
      return desired;
    }

    public IPosition GetPathToEnemy(IPosition current, IPosition enemy, IBattlefield battlefield)
    {
      return GetPath(current, enemy, battlefield);
    }

    public IPosition GetRoamingPath(IPosition current, IBattlefield battlefield)
    {
      var random = new Random();

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
