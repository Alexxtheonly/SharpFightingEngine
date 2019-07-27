using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Features
{
  public interface IEngineFeature
  {
    Guid Id { get; }

    IEnumerable<EngineTick> Apply(IEnumerable<IFighterStats> fighters, EngineCalculationValues calculationValues);
  }
}
