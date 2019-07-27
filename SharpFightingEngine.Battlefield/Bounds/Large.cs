using System;
using System.Numerics;

namespace SharpFightingEngine.Battlefields.Bounds
{
  public class Large : IBounds
  {
    public Guid Id => new Guid("03FDDA5B-A644-4B9D-B257-A02E4D763556");

    public Vector3 Low => new Vector3(0, 0, 0);

    public Vector3 High => new Vector3(500, 500, 0);
  }
}
