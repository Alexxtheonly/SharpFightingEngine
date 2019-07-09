using System.Numerics;

namespace SharpFightingEngine.Battlefields.Bounds
{
  public class Small : IBounds
  {
    public Vector3 Low => new Vector3(0, 0, 0);

    public Vector3 High => new Vector3(100, 100, 0);
  }
}
