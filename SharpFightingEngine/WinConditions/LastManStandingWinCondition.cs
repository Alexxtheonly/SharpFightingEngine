using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.WinConditions
{
  public class LastManStandingWinCondition : IWinCondition
  {
    public Guid Id => new Guid("F5F16639-7796-40EE-B15B-F16EB6E946C4");

    public IMatchResult GetMatchResult(IEnumerable<IFighter> fighters, ICollection<EngineRoundTick> engineRoundTicks)
    {
      return new MatchResult()
      {
        Ticks = engineRoundTicks,
        Wins = fighters.Where(o => o.Health > 0).ToList(),
        Loses = fighters.Where(o => o.Health <= 0).ToList(),
      };
    }

    public bool HasWinner(IEnumerable<IFighterStats> fighters)
    {
      fighters = fighters.Where(o => o.IsAlive());

      if (fighters.Any(o => o.Team != null))
      {
        return fighters
          .GroupBy(o => o.Team)
          .Count() <= 1;
      }

      return fighters
        .Count() <= 1;
    }
  }
}
