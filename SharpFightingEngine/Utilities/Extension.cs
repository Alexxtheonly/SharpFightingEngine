using System;
using System.Collections.Generic;

namespace SharpFightingEngine.Utilities
{
  public static class Extension
  {
    private static readonly Random Random = new Random();

    public static bool Chance(this float chance)
    {
      return chance.Chance(1, 101);
    }

    public static bool Chance(this float chance, int min, int max)
    {
      return Random.Next(min, max) < chance;
    }

    public static IEnumerable<T> Yield<T>(this T item)
    {
      yield return item;
    }
  }
}
