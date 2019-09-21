using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Constants;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.WinConditions
{
  public class LastManStandingWinCondition : IWinCondition
  {
    public Guid Id => WinConditionConstants.LastManStanding;

    public IMatchResult GetMatchResult(IEnumerable<IFighter> fighters, ICollection<EngineRoundTick> engineRoundTicks)
    {
      return new MatchResult()
      {
        Ticks = engineRoundTicks,
      };
    }

    public bool HasWinner(IEnumerable<IFighterStats> fighters, EngineCalculationValues calculationValues)
    {
      fighters = fighters.Where(o => o.IsAlive(calculationValues));

      if (fighters.Any(o => o.Team != null))
      {
        return fighters
          .GroupBy(o => o.Team)
          .Count() <= 1;
      }

      return fighters
        .Count() <= 1;
    }
  }
}
