namespace SharpFightingEngine.Fighters
{
  public interface IUtilityStats
  {
    /// <summary>
    /// Indicates the distance that can be covered in a round.
    /// </summary>
    float Speed { get; set; }

    /// <summary>
    /// Indicates how much energy the fighter has. Energy is used to perform skills.
    /// </summary>
    float Stamina { get; set; }

    /// <summary>
    /// Indicates the fighter's ability to regenerate. Regeneration restores life points every round.
    /// </summary>
    float Regeneration { get; set; }

    /// <summary>
    /// Indicates the vision of the fighter. The vision affects the distance the enemy can be seen at.
    /// </summary>
    float Vision { get; set; }
  }
}
