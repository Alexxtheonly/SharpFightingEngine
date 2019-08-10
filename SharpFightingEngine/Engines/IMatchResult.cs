using System.Collections.Generic;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Engines
{
  public interface IMatchResult
  {
    IEnumerable<EngineRoundTick> Ticks { get; }

    IEnumerable<FighterMatchScore> Scores { get; }

    IEnumerable<TeamMatchScore> TeamScores { get; }
  }
}
