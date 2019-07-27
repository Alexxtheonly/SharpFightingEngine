using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Features
{
  public class FeatureRegenerateEnergy : IEngineFeature
  {
    public Guid Id => new Guid("77C70366-24FB-4AF3-8A34-869F930BC420");

    public IEnumerable<EngineTick> Apply(IEnumerable<IFighterStats> fighters, EngineCalculationValues calculationValues)
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
