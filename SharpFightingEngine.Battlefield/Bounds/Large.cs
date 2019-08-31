using System;
using System.Numerics;
using SharpFightingEngine.Battlefields.Constants;

namespace SharpFightingEngine.Battlefields.Bounds
{
  public class Large : IBounds
  {
    public Guid Id => BoundsConstants.Large;

    public Vector3 Low => new Vector3(0, 0, 0);

    public Vector3 High => new Vector3(500, 500, 0);
  }
}
