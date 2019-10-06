using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Constants;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Features
{
  public class FeatureReviveDeadFighters : IEngineFeature
  {
    public Guid Id => FeatureConstants.ReviveDeadFighters;

    public bool NeedsUpdatedDeadFighters => true;

    public IEnumerable<EngineTick> Apply(Dictionary<Guid, IFighterStats> aliveFighters, Dictionary<Guid, IFighterStats> deadFighters, IEnumerable<EngineRoundTick> rounds, EngineConfiguration configuration)
    {
      foreach (var deadFighter in deadFighters.ToList())
      {
        aliveFighters.Add(deadFighter.Key, deadFighter.Value);
        deadFighters.Remove(deadFighter.Key);

        deadFighter.Value.DamageTaken = 0;
        deadFighter.Value.Health = deadFighter.Value.HealthRemaining(configuration.CalculationValues);

        configuration.PositionGenerator.SetFighterPosition(deadFighter.Value, configuration.Battlefield);

        yield return new FighterSpawnTick()
        {
          Fighter = deadFighter.Value.AsStruct(),
        };
      }
    }
  }
}
