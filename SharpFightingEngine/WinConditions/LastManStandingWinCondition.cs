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
      var scores = OrderScores(engineRoundTicks.CalculateFighterMatchScores());

      return new MatchResult()
      {
        Ticks = engineRoundTicks,
        Scores = scores,
        TeamScores = OrderScores(scores.CalculateTeamMatchScores()),
      };
    }

    public bool HasWinner(IEnumerable<IFighterStats> fighters, IEnumerable<EngineRoundTick> roundTicks, EngineCalculationValues calculationValues)
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

    private IEnumerable<T> OrderScores<T>(IEnumerable<T> scores)
      where T : IMatchScore
    {
      return scores
        .OrderBy(o => o.TotalDeaths)
        .ThenByDescending(o => o.TotalKills)
        .ThenByDescending(o => o.TotalAssists)
        .ThenByDescending(o => o.RoundsAlive)
        .ThenByDescending(o => o.TotalDamageDone);
    }
  }
}
