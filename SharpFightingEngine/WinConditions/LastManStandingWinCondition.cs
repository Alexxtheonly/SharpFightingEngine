using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.WinConditions
{
  public class LastManStandingWinCondition : IWinCondition
  {
    public Guid Id => new Guid("F5F16639-7796-40EE-B15B-F16EB6E946C4");

    public bool HasWinner(IEnumerable<IFighterStats> fighters)
    {
      return fighters
        .Where(o => o.IsAlive())
        .Count() <= 1;
    }
  }
}
