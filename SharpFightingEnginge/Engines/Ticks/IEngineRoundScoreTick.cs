namespace SharpFightingEngine.Engines.Ticks
{
  public interface IEngineRoundScoreTick
  {
    int DamageDone { get; }

    int DamageTaken { get; }

    int Deaths { get; }

    float DistanceTraveled { get; }

    int Energy { get; }

    int EnergyUsed { get; }

    int Health { get; }

    int Kills { get; }

    int RestoredEnergy { get; }

    int RestoredHealth { get; }

    int Round { get; }
  }
}
