using System;
using System.Numerics;

namespace SharpFightingEngine.Battlefields.Bounds
{
  public class Small : IBounds
  {
    public Guid Id => new Guid("FB1698B4-809B-40CD-94D6-0A3B257255C3");

    public Vector3 Low => new Vector3(0, 0, 0);

    public Vector3 High => new Vector3(100, 100, 0);
  }
}
