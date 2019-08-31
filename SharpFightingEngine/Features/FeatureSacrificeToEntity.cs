using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Features
{
  public class FeatureSacrificeToEntity : IEngineFeature
  {
    public Guid Id => new Guid("732A2A25-97A6-4FA0-AE65-96A503F9A1EA");

    public IEnumerable<EngineTick> Apply(IEnumerable<IFighterStats> fighters, IEnumerable<EngineRoundTick> rounds, EngineCalculationValues calculationValues)
    {
      var round10 = rounds.FirstOrDefault(o => o.Round == 10);
      if (round10 == null)
      {
        yield break;
      }

      var sacrifices = round10.ScoreTick
        .OfType<EngineRoundScoreTick>()
        .Where(o => o.EnergyUsed <= 5)
        .Select(o => o.FighterId);

      foreach (var fighter in fighters.Where(o => sacrifices.Any(s => s == o.Id)))
      {
        fighter.DamageTaken += fighter.Health;
        yield return new FighterSacrificedTick()
        {
          Fighter = fighter.AsStruct(),
        };
      }
    }
  }
}
