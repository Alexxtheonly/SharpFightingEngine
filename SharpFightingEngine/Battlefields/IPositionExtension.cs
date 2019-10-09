using System;
using System.Numerics;

namespace SharpFightingEngine.Battlefields
{
  public static class IPositionExtension
  {
    private static readonly Random Random = new Random();

    public static Vector2 GetVector2(this IPosition position)
    {
      return new Vector2(position.X, position.Y);
    }

    public static Vector3 GetVector3(this IPosition position)
    {
      return new Vector3(position.X, position.Y, position.Z);
    }

    public static float GetDistance(this IPosition position, IPosition other)
    {
      return Vector3.Distance(position.GetVector3(), other.GetVector3());
    }

    public static float GetDistance(this IPosition position, Vector3 other)
    {
      return Vector3.Distance(position.GetVector3(), other);
    }

    public static float GetDistanceAbs(this IPosition position, Vector3 other)
    {
      return Math.Abs(position.GetDistance(other));
    }

    public static float GetDistanceAbs(this IPosition position, IPosition other)
    {
      return Math.Abs(position.GetDistance(other));
    }

    public static Vector3 GetDirection(this IPosition position, IPosition desiredPosition, float distance)
    {
      var startPosition = position.GetVector3();
      var endPosition = desiredPosition.GetVector3();

      var calculated = startPosition + (startPosition.GetDirection(endPosition) * distance);

      return calculated;
    }

    public static Vector3 GetDirection(this Vector3 position, Vector3 desiredPosition)
    {
      return Vector3.Normalize(desiredPosition - position);
    }

    public static IPosition CalculateChargePosition(this IPosition actor, IPosition target, float distance)
    {
      return target.CalculatePullPosition(actor, distance);
    }

    public static IPosition CalculateKnockBackPosition(this IPosition actor, IPosition target, float distance)
    {
      IPosition targetPosition;
      if (actor.IsEqualPosition(target))
      {
        // if target pos is equal we calculate a random position
        targetPosition = new Position()
        {
          X = Random.Next((int)(actor.X - distance), (int)(actor.X + distance)),
          Y = Random.Next((int)(actor.Y - distance), (int)(actor.Y + distance)),
          Z = 0,
        };
      }
      else
      {
        targetPosition = target;
      }

      var knockbackVector = actor.GetVector3().GetDirection(targetPosition.GetVector3());

      return (target.GetVector3() + (knockbackVector * distance)).GetPosition();
    }

    public static IPosition CalculatePullPosition(this IPosition actor, IPosition target, float distance)
    {
      const float desiredDistanceAfterPull = 1;

      var distanceBetween = actor.GetDistanceAbs(target);
      var pullDistance = Math.Min(distanceBetween - desiredDistanceAfterPull, distance);

      return target.GetDirection(actor, pullDistance).GetPosition();
    }

    public static bool IsInsideBounds(this IPosition position, IBounds bounds)
    {
      return position.GetVector3().IsInsideBounds(bounds);
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

    public static bool IsEqualPosition(this IPosition position, IPosition other)
    {
      return position.X == other.X &&
        position.Y == other.Y &&
        position.Z == other.Z;
    }
  }
}
