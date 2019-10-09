using System;
using System.Collections.Generic;

namespace SharpFightingEngine.Utilities
{
  public static class Extension
  {
    private static readonly Random Random = new Random();

    public static bool Chance(this float chance)
    {
      return Random.NextDouble() < (chance / 100);
    }

    public static IEnumerable<T> Yield<T>(this T item)
    {
      yield return item;
    }
  }
}
