using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Constants;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills.Buffs;

namespace SharpFightingEngine.Features
{
  public class FeatureApplyBuff : IEngineFeature
  {
    public Guid Id => FeatureConstants.ApplyBuff;

    public bool NeedsUpdatedDeadFighters => false;

    public IEnumerable<EngineTick> Apply(
      Dictionary<Guid, IFighterStats> aliveFighters,
      Dictionary<Guid, IFighterStats> deadFighters,
      IEnumerable<EngineRoundTick> rounds,
      EngineConfiguration configuration)
    {
      foreach (var fighter in aliveFighters.Values)
      {
        foreach (var buff in fighter.States
          .OfType<ISkillBuff>()
          .ToList())
        {
          buff.Remaining -= 1;
          if (buff.Remaining <= 0)
          {
            fighter.States.Remove(buff);
          }

          foreach (var tick in buff.Apply(fighter, buff.Source, configuration.CalculationValues))
          {
            yield return tick;
          }
        }
      }
    }
  }
}
