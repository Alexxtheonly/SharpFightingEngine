using System;
using System.Numerics;
using SharpFightingEngine.Battlefields.Constants;

namespace SharpFightingEngine.Battlefields.Bounds
{
  public class Tiny : IBounds
  {
    public Guid Id => BoundsConstants.Tiny;

    public Vector3 Low => new Vector3(0, 0, 0);

    public Vector3 High => new Vector3(50, 50, 0);
  }
}
