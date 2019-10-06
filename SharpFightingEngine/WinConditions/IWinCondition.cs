using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.WinConditions
{
  public interface IWinCondition : IEndCondition
  {
    Guid Id { get; }

    bool HasWinner(IEnumerable<IFighterStats> fighters, IEnumerable<EngineRoundTick> roundTicks, EngineCalculationValues calculationValues);
  }
}
