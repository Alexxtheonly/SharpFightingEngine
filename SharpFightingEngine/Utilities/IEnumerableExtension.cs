using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpFightingEngine.Utilities
{
  public static class IEnumerableExtension
  {
    private static readonly Random Random = new Random();

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
      return source.Shuffle(Random);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
    {
      if (source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }

      if (rng == null)
      {
        throw new ArgumentNullException(nameof(rng));
      }

      return source.ShuffleIterator(rng);
    }

    public static T GetRandom<T>(this IEnumerable<T> items)
    {
      int index = Random.Next(0, items.Count());

      return items.ElementAt(index);
    }

    private static IEnumerable<T> ShuffleIterator<T>(
        this IEnumerable<T> source, Random rng)
    {
      var buffer = source.ToList();
      for (int i = 0; i < buffer.Count; i++)
      {
        int j = rng.Next(i, buffer.Count);
        yield return buffer[j];

        buffer[j] = buffer[i];
      }
    }
  }
}
