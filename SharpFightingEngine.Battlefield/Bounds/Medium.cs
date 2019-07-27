using System;
using System.Numerics;

namespace SharpFightingEngine.Battlefields.Bounds
{
  public class Medium : IBounds
  {
    public Guid Id => new Guid("B174D408-EA66-4E63-A321-613EA6B4DBBE");

    public Vector3 Low => new Vector3(0, 0, 0);

    public Vector3 High => new Vector3(150, 150, 0);
  }
}
