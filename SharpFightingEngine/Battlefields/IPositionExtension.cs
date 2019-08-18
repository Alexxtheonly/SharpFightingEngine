using System;
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

    public static float GetDistanceAbs(this IPosition position, IPosition other)
    {
      return Math.Abs(position.GetDistance(other));
    }

    public static Vector3 GetDirection(this IPosition position, IPosition desiredPosition, float distance)
    {
      var startPosition = position.GetVector3();
      var endPosition = desiredPosition.GetVector3();

      var calculated = startPosition + (Vector3.Normalize(endPosition - startPosition) * distance);

      return calculated;
    }

    public static bool IsInsideBounds(this IPosition position, IBounds bounds)
    {
      var positionVector = position.GetVector3();

      return Vector3.Clamp(positionVector, bounds.Low, bounds.High) == positionVector;
    }

    public static IPosition Set(this IPosition position, IPosition newposition)
    {
      position.X = newposition.X;
      position.Y = newposition.Y;
      position.Z = newposition.Z;

      return position;
    }

    public static IPosition Set(this IPosition position, Vector3 vector3)
    {
      position.X = vector3.X;
      position.Y = vector3.Y;
      position.Z = vector3.Z;

      return position;
    }
  }
}
