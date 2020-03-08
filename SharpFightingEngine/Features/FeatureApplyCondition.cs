using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Constants;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills.Conditions;

namespace SharpFightingEngine.Features
{
  public class FeatureApplyCondition : IEngineFeature
  {
    public Guid Id => FeatureConstants.ApplyCondition;

    public bool NeedsUpdatedDeadFighters => true;

    public IEnumerable<EngineTick> Apply(
      Dictionary<Guid, IFighterStats> aliveFighters,
      Dictionary<Guid, IFighterStats> deadFighters,
      IEnumerable<EngineRoundTick> rounds,
      EngineConfiguration configuration)
    {
      foreach (var fighter in aliveFighters.Values)
      {
        foreach (var condition in fighter.States
          .OfType<ISkillCondition>()
          .ToList())
        {
          condition.Remaining -= 1;
          if (condition.Remaining <= 0)
          {
            fighter.States.Remove(condition);
          }

          foreach (var tick in condition.Apply(fighter, condition.Source, configuration.CalculationValues))
          {
            yield return tick;
          }
        }
      }
    }
  }
}
