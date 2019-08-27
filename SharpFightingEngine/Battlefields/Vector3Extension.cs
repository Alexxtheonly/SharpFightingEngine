using System;
using System.Collections.Generic;
using System.Numerics;

namespace SharpFightingEngine.Battlefields
{
  public static class Vector3Extension
  {
    public static bool IsInsideBounds(this Vector3 position, IBounds bounds)
    {
      return Vector3.Clamp(position, bounds.Low, bounds.High) == position;
    }

    public static Vector3 GetEscapeVector(this float distance, Vector3 one, Vector3 two)
    {
      var center = (one + two) / 2;

      var a = Vector3.DistanceSquared(one, center);
      var c = Math.Pow(distance, 2);

      var b = Math.Sqrt(a + c);

      return Vector3.Normalize(Vector3.Max(one, two) - Vector3.Min(one, two)) * (float)b;
    }

    public static IPosition GetPosition(this Vector3 vector3)
    {
      var pos = new Position()
      {
      }.Set(vector3);

      return pos;
    }
  }
}
