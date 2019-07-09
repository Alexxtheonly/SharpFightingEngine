using System.Collections.Generic;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Engines
{
  public interface IMatchResult
  {
    IEnumerable<EngineRoundTick> Ticks { get; }

    IEnumerable<MatchScore> Scores { get; }

    IEnumerable<MatchScore> TeamScores { get; }
  }
}
