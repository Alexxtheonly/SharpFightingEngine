using System;

namespace SharpFightingEngine.Engines
{
  public struct FighterMatchScore : IMatchScore
  {
    public Guid Id { get; set; }

    public Guid? TeamId { get; set; }

    public int MaxHealth { get; set; }

    public int TotalDamageDone { get; set; }

    public int TotalDamageTaken { get; set; }

    public int TotalKills { get; set; }

    public int TotalDeaths { get; set; }

    public float TotalDistanceTraveled { get; set; }

    public int RoundsAlive { get; set; }

    public int TotalHealingDone { get; set; }

    public int TotalHealingRecieved { get; set; }
  }
}
