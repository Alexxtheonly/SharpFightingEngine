namespace SharpFightingEngine.Fighters
{
  public interface IUtilityStats
  {
    /// <summary>
    /// Indicates the distance that can be covered in a round.
    /// </summary>
    int Speed { get; set; }

    /// <summary>
    /// Indicates the vision of the fighter. The vision affects the distance the enemy can be seen at.
    /// </summary>
    int Vision { get; set; }

    int Level { get; set; }
  }
}
