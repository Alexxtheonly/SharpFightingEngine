namespace SharpFightingEngine.Fighters
{
  public interface IDefensiveStats
  {
    /// <summary>
    /// Indicates the mobility of the fighter. Mobility increases the chance of avoiding enemy attacks.
    /// </summary>
    float Agility { get; }

    /// <summary>
    /// Indicates the armor of the fighter. Armor reduces the damage taken by enemy attacks.
    /// </summary>
    float Toughness { get; }

    /// <summary>
    /// Indicates how many life points the fighter has.
    /// </summary>
    float Vitality { get; }
  }
}
