using System;
using System.Numerics;

namespace SharpFightingEngine.Battlefields.Bounds
{
  public class Big : IBounds
  {
    public Guid Id => new Guid("03B94283-D252-4842-B224-724422671CDC");

    public Vector3 Low => new Vector3(0, 0, 0);

    public Vector3 High => new Vector3(250, 250, 0);
  }
}
