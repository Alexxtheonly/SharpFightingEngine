namespace SharpFightingEngine.Fighters
{
  public interface IOffensiveStats
  {
    /// <summary>
    /// Indicates the accuracy of the fighter. Accuracy reduces the chance of missing an opponent with an attack.
    /// </summary>
    float Accuracy { get; set; }

    /// <summary>
    /// Indicates the power of the fighter. Power increases the damage caused by abilities.
    /// </summary>
    float Power { get; set; }

    /// <summary>
    /// Indicates the expertise. Expertise increases chance of critical hits with attacks.
    /// </summary>
    float Expertise { get; set; }
  }
}
