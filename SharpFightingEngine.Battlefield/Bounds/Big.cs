using System;
using System.Numerics;
using SharpFightingEngine.Battlefields.Constants;

namespace SharpFightingEngine.Battlefields.Bounds
{
  public class Big : IBounds
  {
    public Guid Id => BoundsConstants.Big;

    public Vector3 Low => new Vector3(0, 0, 0);

    public Vector3 High => new Vector3(250, 250, 0);
  }
}
