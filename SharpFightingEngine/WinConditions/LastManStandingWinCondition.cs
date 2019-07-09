using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.WinConditions
{
  public class LastManStandingWinCondition : IWinCondition
  {
    /// <summary>
    /// Indicates after how many rounds, without significant change, it is a draw.
    /// </summary>
    private const int ConsideredStaleAfterRounds = 20;

    private Queue<Tuple<int, IEnumerable<IFighter>>> lastRoundsQueue = new Queue<Tuple<int, IEnumerable<IFighter>>>(ConsideredStaleAfterRounds);

    public bool HasWinner(IEnumerable<IFighterStats> fighters)
    {
      return fighters
        .Where(o => o.IsAlive())
        .Count() <= 1;
    }

    public bool IsDraw(IEnumerable<IFighterStats> fighters, int round)
    {
      lastRoundsQueue.Enqueue(Tuple.Create<int, IEnumerable<IFighter>>(round, fighters.Select(o => o.AsStruct()).ToList()));

      if (lastRoundsQueue.Count != ConsideredStaleAfterRounds)
      {
        return false;
      }

      return !lastRoundsQueue
         .Dequeue()
         .Item2
         .Select(o => o.Id)
         .Except(fighters.Select(o => o.Id))
         .Any();
    }
  }
}
