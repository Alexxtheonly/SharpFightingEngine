using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Features
{
  public class FeatureRegenerateHealth : IEngineFeature
  {
    public IEnumerable<EngineTick> Apply(IEnumerable<IFighterStats> fighters, EngineCalculationValues calculationValues)
    {
      foreach (var fighter in fighters)
      {
        var tick = fighter.RegenerateHealth(calculationValues);

        if (tick != null)
        {
          yield return tick;
        }
      }
    }
  }
}
