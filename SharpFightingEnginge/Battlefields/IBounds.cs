using System.Numerics;

namespace SharpFightingEngine.Battlefields
{
  public interface IBounds
  {
    Vector3 Low { get; }

    Vector3 High { get; }
  }
}
