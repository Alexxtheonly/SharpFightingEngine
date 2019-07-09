using System;

namespace SharpFightingEngine.Utilities
{
  public static class Extension
  {
    private static readonly Random Random = new Random();

    public static bool Chance(this float chance)
    {
      return chance.Chance(0, 100);
    }

    public static bool Chance(this float chance, int min, int max)
    {
      return Random.Next(min, max) < chance;
    }
  }
}
