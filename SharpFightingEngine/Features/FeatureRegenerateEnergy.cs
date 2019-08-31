using System;
using System.Collections.Generic;
using SharpFightingEngine.Constants;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Features
{
  public class FeatureRegenerateEnergy : IEngineFeature
  {
    public Guid Id => FeatureConstants.RegenerateEnergy;

    public IEnumerable<EngineTick> Apply(IEnumerable<IFighterStats> fighters, IEnumerable<EngineRoundTick> rounds, EngineCalculationValues calculationValues)
    {
      foreach (var fighter in fighters)
      {
        var tick = fighter.RegenerateEnergy(calculationValues);

        if (tick != null)
        {
          yield return tick;
        }
      }
    }
  }
}
