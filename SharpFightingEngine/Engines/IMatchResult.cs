using System.Collections.Generic;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Engines
{
  public interface IMatchResult
  {
    IEnumerable<EngineRoundTick> Ticks { get; }

    IEnumerable<FighterMatchScore> Scores { get; }

    IEnumerable<TeamMatchScore> TeamScores { get; }

    ICollection<IFighter> Wins { get; set; }

    ICollection<IFighter> Draws { get; set; }

    ICollection<IFighter> Loses { get; set; }
  }
}
