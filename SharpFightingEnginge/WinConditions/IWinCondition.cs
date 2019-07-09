using System.Collections.Generic;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.WinConditions
{
  public interface IWinCondition
  {
    bool HasWinner(IEnumerable<IFighterStats> fighters);

    bool IsDraw(IEnumerable<IFighterStats> fighters, int round);
  }
}
