using System;
using System.Numerics;
using SharpFightingEngine.Battlefields.Constants;

namespace SharpFightingEngine.Battlefields.Bounds
{
  public class Medium : IBounds
  {
    public Guid Id => BoundsConstants.Medium;

    public Vector3 Low => new Vector3(0, 0, 0);

    public Vector3 High => new Vector3(150, 150, 0);
  }
}
