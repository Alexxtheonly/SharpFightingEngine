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

    public IEnumerable<EngineTick> Apply(IEnumerable<IFighterStats> fighters, IEnumerable<EngineRoundTick> rounds, EngineCalculationValues calculationValues)
    {
      foreach (var fighter in fighters)
      {
        foreach (var buff in fighter.States.OfType<ISkillBuff>())
        {
          foreach (var tick in buff.Apply(fighter, buff.Source, calculationValues))
          {
            yield return tick;
          }
        }
      }
    }
  }
}
