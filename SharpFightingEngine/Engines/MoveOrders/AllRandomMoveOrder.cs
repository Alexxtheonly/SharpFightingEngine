using System.Collections.Generic;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Engines.MoveOrders
{
  public class AllRandomMoveOrder : IMoveOrder
  {
    private IEnumerable<IFighterStats> ordered;

    public IEnumerable<IFighterStats> Init(IEnumerable<IFighterStats> fighters)
    {
      ordered = fighters.Shuffle();
      return ordered;
    }

    public IEnumerable<IFighterStats> Next(IEnumerable<IFighterStats> fighters)
    {
      return ordered;
    }
  }
}
