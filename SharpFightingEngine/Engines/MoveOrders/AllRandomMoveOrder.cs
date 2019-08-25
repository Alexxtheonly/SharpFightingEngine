using System;
using System.Collections.Generic;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Engines.MoveOrders
{
  public class AllRandomMoveOrder : IMoveOrder
  {
    public Guid Id => new Guid("12E9E0AE-ECA3-440D-A649-48D687F6D97B");

    public IEnumerable<IFighterStats> Init(IEnumerable<IFighterStats> fighters)
    {
      return fighters.Shuffle();
    }

    public IEnumerable<IFighterStats> Next(IEnumerable<IFighterStats> fighters)
    {
      return fighters.Shuffle();
    }
  }
}
