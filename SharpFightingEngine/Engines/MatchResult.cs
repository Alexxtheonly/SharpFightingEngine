using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Engines
{
  public class MatchResult : IMatchResult
  {
    public IEnumerable<EngineRoundTick> Ticks { get; set; }

    public IEnumerable<FighterMatchScore> Scores => Ticks
      .SelectMany(o => o.ScoreTick)
      .OrderByDescending(o => o.Round)
      .OfType<EngineRoundScoreTick>()
      .GroupBy(o => o.FighterId)
      .CreateFighterMatchScores()
      .OrderScores();

    public IEnumerable<TeamMatchScore> TeamScores => Ticks
      .SelectMany(o => o.ScoreTick)
      .OrderByDescending(o => o.Round)
      .OfType<EngineRoundTeamScoreTick>()
      .GroupBy(o => o.TeamId)
      .CreateTeamMatchScores()
      .OrderScores();
  }
}
