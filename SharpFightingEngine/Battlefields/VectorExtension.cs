using System.Numerics;

namespace SharpFightingEngine.Battlefields
{
  public static class VectorExtension
  {
    public static bool IsInsideBounds(this Vector3 position, IBounds bounds)
    {
      return Vector3.Clamp(position, bounds.Low, bounds.High) == position;
    }

    public static Vector2 GetEscapeVector(this float distance, Vector2 one, Vector2 two)
    {
      var center = (one + two) / 2;

      // rotate by 90° which equals to 1.5708F radians
      var rotated = Vector2.Transform(center, Matrix3x2.CreateRotation(1.5708F));

      var escapeVector = Vector2.Normalize(rotated) * distance;

      return escapeVector;
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
