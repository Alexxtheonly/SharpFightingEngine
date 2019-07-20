using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.StaleConditions
{
  public class NoWinnerCanBeDeterminedStaleCondition : IStaleCondition
  {
    private const int MaxStaleRounds = 10;

    private int staleCounter = 0;

    public bool IsStale(IEnumerable<IFighterStats> fighters, IEnumerable<EngineRoundTick> roundTicks)
    {
      var previousRound = roundTicks
        .FirstOrDefault(o => o.Round == (roundTicks.Max(u => u.Round) - 1));
      if (previousRound == null)
      {
        return false;
      }

      var attackTicks = previousRound.Ticks.OfType<FighterAttackTick>();
      if (!attackTicks.Any())
      {
        staleCounter++;
      }
      else
      {
        var mortals = previousRound.ScoreTick
          .Where(o => o.DamageTaken > o.RestoredHealth);

        if (!mortals.Any())
        {
          staleCounter++;
        }
      }

      return staleCounter > MaxStaleRounds;
    }
  }
}
