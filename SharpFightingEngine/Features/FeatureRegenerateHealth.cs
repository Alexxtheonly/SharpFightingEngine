using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Features
{
  public class FeatureRegenerateHealth : IEngineFeature
  {
    public Guid Id => new Guid("E800723C-6324-47AB-9593-1952346AD772");

    public IEnumerable<EngineTick> Apply(IEnumerable<IFighterStats> fighters, IEnumerable<EngineRoundTick> rounds, EngineCalculationValues calculationValues)
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
