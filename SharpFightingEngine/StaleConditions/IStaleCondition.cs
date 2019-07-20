using System.Collections.Generic;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.StaleConditions
{
  public interface IStaleCondition
  {
    bool IsStale(IEnumerable<IFighterStats> fighters, IEnumerable<EngineRoundTick> roundTicks);
  }
}
