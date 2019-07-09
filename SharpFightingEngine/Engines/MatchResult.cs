using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Engines
{
  public class MatchResult : IMatchResult
  {
    public IEnumerable<EngineRoundTick> Ticks { get; set; }

    public IEnumerable<MatchScore> Scores => Ticks
      .SelectMany(o => o.ScoreTick)
      .OrderByDescending(o => o.Round)
      .OfType<EngineRoundScoreTick>()
      .GroupBy(o => o.FighterId)
      .CreateMatchScores()
      .OrderScores();

    public IEnumerable<MatchScore> TeamScores => Ticks
      .SelectMany(o => o.ScoreTick)
      .OrderByDescending(o => o.Round)
      .OfType<EngineRoundTeamScoreTick>()
      .GroupBy(o => o.TeamId)
      .CreateMatchScores()
      .OrderScores();
  }
}
