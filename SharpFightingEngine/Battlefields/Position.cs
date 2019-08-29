using System.Numerics;

namespace SharpFightingEngine.Battlefields
{
  public struct Position : IPosition
  {
    public Position(float x, float y)
    {
      X = x;
      Y = y;
      Z = 0;
    }

    public Position(Vector3 vector)
    {
      X = vector.X;
      Y = vector.Y;
      Z = vector.Z;
    }

    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }
  }
}
