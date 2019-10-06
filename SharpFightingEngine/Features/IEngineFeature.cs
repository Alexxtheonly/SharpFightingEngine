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

    bool NeedsUpdatedDeadFighters { get; }

    IEnumerable<EngineTick> Apply(Dictionary<Guid, IFighterStats> aliveFighters, Dictionary<Guid, IFighterStats> deadFighters, IEnumerable<EngineRoundTick> rounds, EngineConfiguration configuration);
  }
}
