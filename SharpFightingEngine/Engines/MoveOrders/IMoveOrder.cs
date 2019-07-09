using System.Collections.Generic;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Engines.MoveOrders
{
  public interface IMoveOrder
  {
    IEnumerable<IFighterStats> Init(IEnumerable<IFighterStats> fighters);

    IEnumerable<IFighterStats> Next(IEnumerable<IFighterStats> fighters);
  }
}
