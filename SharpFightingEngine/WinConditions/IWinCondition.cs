using System;
using System.Collections.Generic;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.WinConditions
{
  public interface IWinCondition
  {
    Guid Id { get; }

    bool HasWinner(IEnumerable<IFighterStats> fighters);
  }
}
