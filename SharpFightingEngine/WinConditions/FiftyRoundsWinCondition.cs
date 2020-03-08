using System;
using System.Collections.Generic;
using System.Linq;
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
      return roundTicks.GetLastRound()?.Round == 50;
    }

    private IEnumerable<T> OrderScores<T>(IEnumerable<T> scores)
      where T : IMatchScore
    {
      return scores
        .OrderByDescending(o => o.TotalKills)
        .OrderByDescending(o => o.TotalAssists)
        .ThenBy(o => o.TotalDeaths)
        .ThenByDescending(o => o.TotalDamageDone);
    }
  }
}
