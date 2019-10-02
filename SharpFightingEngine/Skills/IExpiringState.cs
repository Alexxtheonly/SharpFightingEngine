using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills
{
  public interface IExpiringState
  {
    Guid Id { get; }

    string Name { get; }

    int Remaining { get; set; }

    int Initial { get; }

    IFighterStats Source { get; }

    void Apply(IStats stats);

    IEnumerable<EngineTick> Apply(IFighterStats target, IFighterStats source, EngineCalculationValues calculationValues);
  }
}
