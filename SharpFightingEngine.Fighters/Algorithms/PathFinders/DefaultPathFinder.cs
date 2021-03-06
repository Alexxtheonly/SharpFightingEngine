﻿using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;

namespace SharpFightingEngine.Fighters.Algorithms.PathFinders
{
  public class DefaultPathFinder : IPathFinder
  {
    private readonly Random random = new Random();

    public IPosition GetEscapePath(IPosition current, IEnumerable<IPosition> escape, IBattlefield battlefield)
    {
      const float desiredDistance = 20;

      var orderedByDistance = escape
        .Select(o => new { Position = o, Distance = current.GetDistance(o) })
        .OrderByDescending(o => o.Distance);

      var low = orderedByDistance.First();
      var high = orderedByDistance.Last();

      var tuples = current
        .GetVector2()
        .GetDistancesTo(low.Position.GetVector2(), high.Position.GetVector2(), desiredDistance)
        .OrderByDescending(o => o.Item3);

      var escapeVectors = tuples
        .Where(o => o.Item1.AsVector3().IsInsideBounds(battlefield.CurrentBounds));

      var best = escapeVectors
        .FirstOrDefault();

      if (best == null)
      {
        best = tuples.First();
      }

      return best
        .Item1
        .AsVector3()
        .Clamp(battlefield.CurrentBounds.Low, battlefield.CurrentBounds.High)
        .GetPosition();
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
