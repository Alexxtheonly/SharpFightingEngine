using System.Collections.Generic;
using System.Linq;

namespace SharpFightingEngine.Engines.Ticks
{
  public static class EngineTickExtension
  {
    public static IEnumerable<EngineTick> GetRounds(this IEnumerable<EngineRoundTick> roundTicks, int from, int to)
    {
      return roundTicks
        .Where(o => o.Round >= from && o.Round <= to)
        .SelectMany(o => o.Ticks);
    }

    public static EngineRoundTick GetLastRound(this IEnumerable<EngineRoundTick> roundTicks)
    {
      return roundTicks
        .OrderByDescending(o => o.Round)
        .FirstOrDefault();
    }

    public static IEnumerable<EngineTick> GetLastRounds(this IEnumerable<EngineRoundTick> roundTicks, int rounds)
    {
      var maxRound = roundTicks.Max(o => o.Round);

      return roundTicks.GetRounds(maxRound - rounds, maxRound);
    }
  }
}
