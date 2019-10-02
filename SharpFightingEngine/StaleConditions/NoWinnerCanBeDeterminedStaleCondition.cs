using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Constants;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.StaleConditions
{
  public class NoWinnerCanBeDeterminedStaleCondition : IStaleCondition
  {
    private const int MaxStaleRounds = 25;
    private const int MaxRounds = 100;

    private int staleCounter = 0;

    public Guid Id => StaleConditionConstants.NoWinnerCanBeDetermined;

    public IMatchResult GetMatchResult(IEnumerable<IFighter> fighters, ICollection<EngineRoundTick> engineRoundTicks)
    {
      return new MatchResult()
      {
        Ticks = engineRoundTicks,
      };
    }

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

      return staleCounter > MaxStaleRounds || roundTicks.GetLastRound().Round >= MaxRounds;
    }
  }
}
