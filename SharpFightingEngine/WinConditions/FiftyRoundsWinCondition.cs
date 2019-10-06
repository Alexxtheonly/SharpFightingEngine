using System;
using System.Collections.Generic;
using SharpFightingEngine.Constants;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.WinConditions
{
  public class FiftyRoundsWinCondition : IWinCondition
  {
    public Guid Id => WinConditionConstants.FiftyRounds;

    public IMatchResult GetMatchResult(IEnumerable<IFighter> fighters, ICollection<EngineRoundTick> engineRoundTicks)
    {
      return new MatchResult()
      {
        Ticks = engineRoundTicks,
      };
    }

    public bool HasWinner(IEnumerable<IFighterStats> fighters, IEnumerable<EngineRoundTick> roundTicks, EngineCalculationValues calculationValues)
    {
      return roundTicks.GetLastRound()?.Round == 50;
    }
  }
}
