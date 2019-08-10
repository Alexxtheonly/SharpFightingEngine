using System.Collections.Generic;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Engines
{
  public interface IEndCondition
  {
    IMatchResult GetMatchResult(IEnumerable<IFighter> fighters, ICollection<EngineRoundTick> engineRoundTicks);
  }
}
