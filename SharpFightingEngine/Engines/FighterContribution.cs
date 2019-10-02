using System;

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
    /// The kills and assists in this match. At least 25% of the target's health must be dealt damage.
    /// </summary>
    public int KillsAndAssists { get; set; }

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
