using System.Collections.Generic;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Fighters
{
  public interface IFighterStats : IFighter, IStats
  {
    IFighterAction GetFighterAction(IEnumerable<IFighterStats> visibleFighters, IBattlefield battlefield, IEnumerable<EngineRoundTick> roundTicks, EngineCalculationValues calculationValues);
  }
}
