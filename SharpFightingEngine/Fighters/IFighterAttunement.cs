using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Fighters
{
  public interface IFighterAttunement
  {
    IEnumerable<EngineTick> Attack(IFighterStats actor, IFighterStats target, EngineCalculationValues calculationValues);

    IEnumerable<EngineTick> Effect(IFighterStats fighter, EngineCalculationValues calculationValues);

    int CalculateDamageDone(IFighterAttunement target, int currentDamage);
  }
}
