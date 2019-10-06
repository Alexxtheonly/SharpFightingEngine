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

    public bool NeedsUpdatedDeadFighters => false;

    public IEnumerable<EngineTick> Apply(
      Dictionary<Guid, IFighterStats> aliveFighters,
      Dictionary<Guid, IFighterStats> deadFighters,
      IEnumerable<EngineRoundTick> rounds,
      EngineConfiguration configuration)
    {
      var sacrifices = aliveFighters
        .Values
        .Where(o => (o.Stats.DefensivePowerLevel() / o.Stats.PowerLevel()) > 0.7);

      foreach (var sacrifice in sacrifices)
      {
        sacrifice.DamageTaken += sacrifice.HealthRemaining(configuration.CalculationValues);

        yield return new FighterSacrificedTick()
        {
          Fighter = sacrifice,
        };
      }
    }
  }
}
