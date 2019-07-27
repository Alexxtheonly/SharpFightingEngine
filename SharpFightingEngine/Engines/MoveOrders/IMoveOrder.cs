using System;
using System.Collections.Generic;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Engines.MoveOrders
{
  public interface IMoveOrder
  {
    Guid Id { get; }

    IEnumerable<IFighterStats> Init(IEnumerable<IFighterStats> fighters);

    IEnumerable<IFighterStats> Next(IEnumerable<IFighterStats> fighters);
  }
}
