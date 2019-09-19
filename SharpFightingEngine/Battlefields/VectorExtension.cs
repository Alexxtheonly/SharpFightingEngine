using System;
using System.Collections.Generic;
using System.Numerics;

namespace SharpFightingEngine.Battlefields
{
  public static class VectorExtension
  {
    public static bool IsInsideBounds(this Vector3 position, IBounds bounds)
    {
      return Vector3.Clamp(position, bounds.Low, bounds.High) == position;
    }

    public static IEnumerable<Tuple<Vector2, float, float>> GetDistancesTo(this Vector2 position, Vector2 one, Vector2 two, float distance)
    {
      var normalized = Vector2.Normalize(position);

      for (double i = 0; i < Math.PI * 2; i += Math.PI / 12)
      {
        for (int u = 1; u < 5; u++)
        {
          var rotated = Vector2.Transform(normalized, Matrix4x4.CreateRotationY((float)i));
          var adjustedDistance = distance / u;
          var escape = position + (rotated * adjustedDistance);

          yield return Tuple.Create(escape, adjustedDistance, escape.GetDistanceAbs(one) + escape.GetDistanceAbs(two));
        }
      }
    }

    public static Vector2 GetDirection(this Vector2 start, Vector2 end, float distance)
    {
      return start + (Vector2.Normalize(end - start) * distance);
    }

    public static float GetDistance(this Vector2 position, Vector2 other)
    {
      return Vector2.Distance(position, other);
    }

    public static float GetDistanceAbs(this Vector2 position, Vector2 other)
    {
      return Math.Abs(position.GetDistance(other));
    }

    public static IPosition GetPosition(this Vector2 vector2)
    {
      return new Position()
      {
        X = vector2.X,
        Y = vector2.Y,
      };
    }

    public static IPosition GetPosition(this Vector3 vector3)
    {
      var pos = new Position()
      {
      }.Set(vector3);

      return pos;
    }

    public static Vector3 AsVector3(this Vector2 vector)
    {
      return new Vector3(vector, 0);
    }

    public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max)
    {
      return Vector3.Clamp(vector, min, max);
    }
  }
}
