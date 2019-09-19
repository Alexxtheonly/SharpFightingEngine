using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Constants;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Features
{
  public class FeatureSacrificeToEntity : IEngineFeature
  {
    public Guid Id => FeatureConstants.SacrificeToEntity;

    public IEnumerable<EngineTick> Apply(IEnumerable<IFighterStats> fighters, IEnumerable<EngineRoundTick> rounds, EngineCalculationValues calculationValues)
    {
      var sacrifices = fighters
        .Where(o => (o.Stats.DefensivePowerLevel() / o.Stats.PowerLevel()) > 0.7);

      foreach (var sacrifice in sacrifices)
      {
        sacrifice.DamageTaken += sacrifice.HealthRemaining(calculationValues);

        yield return new FighterSacrificedTick()
        {
          Fighter = sacrifice,
        };
      }
    }
  }
}
