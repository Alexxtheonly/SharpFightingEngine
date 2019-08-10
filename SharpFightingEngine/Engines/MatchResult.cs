using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Engines
{
  public class MatchResult : IMatchResult
  {
    public IEnumerable<EngineRoundTick> Ticks { get; set; }

    public ICollection<IFighter> Wins { get; set; } = new List<IFighter>();

    public ICollection<IFighter> Draws { get; set; } = new List<IFighter>();

    public ICollection<IFighter> Loses { get; set; } = new List<IFighter>();

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
