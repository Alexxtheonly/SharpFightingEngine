using System;
using System.Numerics;

namespace SharpFightingEngine.Battlefields.Bounds
{
  public class Tiny : IBounds
  {
    public Guid Id => new Guid("86B56A4F-77F4-4624-B67E-1887E77039A0");

    public Vector3 Low => new Vector3(0, 0, 0);

    public Vector3 High => new Vector3(50, 50, 0);
  }
}
