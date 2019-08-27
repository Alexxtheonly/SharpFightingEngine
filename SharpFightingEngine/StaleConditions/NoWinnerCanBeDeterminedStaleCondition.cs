using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.StaleConditions
{
  public class NoWinnerCanBeDeterminedStaleCondition : IStaleCondition
  {
    private const int MaxStaleRounds = 20;
    private const int MaxRounds = 250;

    private int staleCounter = 0;

    public Guid Id => new Guid("04616688-2CD1-4341-B757-AFDAE8AF4035");

    public IMatchResult GetMatchResult(IEnumerable<IFighter> fighters, ICollection<EngineRoundTick> engineRoundTicks)
    {
      return new MatchResult()
      {
        Ticks = engineRoundTicks,
        Draws = fighters.Where(o => o.Health > 0).ToList(),
        Loses = fighters.Where(o => o.Health <= 0).ToList(),
      };
    }

    public bool IsStale(IEnumerable<IFighterStats> fighters, IEnumerable<EngineRoundTick> roundTicks)
    {
      var previousRound = roundTicks
        .FirstOrDefault(o => o.Round == (roundTicks.Max(u => u.Round) - 1));
      if (previousRound == null)
      {
        return false;
      }

      var attackTicks = previousRound.Ticks.OfType<FighterAttackTick>();
      if (!attackTicks.Any())
      {
        staleCounter++;
      }
      else
      {
        var mortals = previousRound.ScoreTick
          .Where(o => o.DamageTaken > o.RestoredHealth);

        if (!mortals.Any())
        {
          staleCounter++;
        }
      }

      return staleCounter > MaxStaleRounds || roundTicks.GetMaxRound().Round >= MaxRounds;
    }
  }
}
