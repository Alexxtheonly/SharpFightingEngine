using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Constants;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Features
{
  public class FeatureInvokeAttunementEffect : IEngineFeature
  {
    public Guid Id => FeatureConstants.InvokeAttunementEffect;

    public bool NeedsUpdatedDeadFighters => false;

    public IEnumerable<EngineTick> Apply(Dictionary<Guid, IFighterStats> aliveFighters, Dictionary<Guid, IFighterStats> deadFighters, IEnumerable<EngineRoundTick> rounds, EngineConfiguration configuration)
    {
      foreach (var aliveFighter in aliveFighters.Values.Where(o => o.Attunement != null))
      {
        foreach (var tick in aliveFighter.Attunement.Effect(aliveFighter, configuration.CalculationValues))
        {
          yield return tick;
        }
      }
    }
  }
}
