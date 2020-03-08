using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpFightingEngine.Engines
{
  public class FighterContribution
  {
    public Guid FighterId { get; set; }

    /// <summary>
    /// Is true if the fighter has won the match.
    /// </summary>
    public bool HasWon { get; set; }

    /// <summary>
    /// Is true if the fighter finished second place
    /// </summary>
    public bool IsSecond { get; set; }

    /// <summary>
    /// Is true if the fighter finished third place
    /// </summary>
    public bool IsThird { get; set; }

    /// <summary>
    /// This collection contains the fighter ids that were killed by this fighter
    /// </summary>
    public IEnumerable<Guid> Kills { get; set; } = Enumerable.Empty<Guid>();

    /// <summary>
    /// This collection contains the fighter ids that this fighter assisted killing
    /// </summary>
    public IEnumerable<Guid> Assists { get; set; } = Enumerable.Empty<Guid>();

    /// <summary>
    /// The percentage of how many of the total rounds the fighter has been alive.
    /// </summary>
    public double PercentageOfRoundsAlive { get; set; }

    /// <summary>
    /// The match participation, indicates the relationship between active and passive actions.
    /// </summary>
    public double MatchParticipation { get; set; }
  }
}
