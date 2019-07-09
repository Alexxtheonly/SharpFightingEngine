using System.Numerics;

namespace SharpFightingEngine.Battlefields
{
  public static class IPositionExtension
  {
    public static Vector3 GetVector3(this IPosition position)
    {
      return new Vector3(position.X, position.Y, position.Z);
    }

    public static float GetDistance(this IPosition position, IPosition other)
    {
      return Vector3.Distance(position.GetVector3(), other.GetVector3());
    }

    public static bool IsInsideBounds(this IPosition position, IBounds bounds)
    {
      var positionVector = position.GetVector3();

      return Vector3.Clamp(positionVector, bounds.Low, bounds.High) == positionVector;
    }

    public static void Set(this IPosition position, IPosition newposition)
    {
      position.X = newposition.X;
      position.Y = newposition.Y;
      position.Z = newposition.Z;
    }

    public static void Set(this IPosition position, Vector3 vector3)
    {
      position.X = vector3.X;
      position.Y = vector3.Y;
      position.Z = vector3.Z;
    }
  }
}
