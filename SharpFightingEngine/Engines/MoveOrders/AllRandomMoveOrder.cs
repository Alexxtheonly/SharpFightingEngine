using System;
using System.Collections.Generic;
using SharpFightingEngine.Constants;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Engines.MoveOrders
{
  public class AllRandomMoveOrder : IMoveOrder
  {
    public Guid Id => MoveOrderConstants.AllRandom;

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
