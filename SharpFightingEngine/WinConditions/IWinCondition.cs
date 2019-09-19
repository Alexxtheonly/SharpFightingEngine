using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.WinConditions
{
  public interface IWinCondition : IEndCondition
  {
    Guid Id { get; }

    bool HasWinner(IEnumerable<IFighterStats> fighters, EngineCalculationValues calculationValues);
  }
}
