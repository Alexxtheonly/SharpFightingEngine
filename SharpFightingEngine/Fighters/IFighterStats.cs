using System.Collections.Generic;
using SharpFightingEngine.Battlefields;

namespace SharpFightingEngine.Fighters
{
  public interface IFighterStats : IFighter, IStats
  {
    IFighterAction GetFighterAction(IEnumerable<IFighterStats> visibleFighters, IBattlefield battlefield);
  }
}
