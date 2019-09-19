namespace SharpFightingEngine.Fighters
{
  public struct Stats : IStats
  {
    /// <summary>
    /// Indicates the distance that can be covered in a round.
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    /// Indicates how much energy the fighter has. Energy is used to perform skills.
    /// </summary>
    public float Stamina { get; set; }

    /// <summary>
    /// Indicates how many life points the fighter has.
    /// </summary>
    public float Vitality { get; set; }

    /// <summary>
    /// Indicates the power of the fighter. Power increases the damage caused by abilities.
    /// </summary>
    public float Power { get; set; }

    /// <summary>
    /// Indicates the mobility of the fighter. Mobility increases the chance of avoiding enemy attacks.
    /// </summary>
    public float Agility { get; set; }

    /// <summary>
    /// Indicates the accuracy of the fighter. Accuracy reduces the chance of missing an opponent with an attack.
    /// </summary>
    public float Accuracy { get; set; }

    /// <summary>
    /// Indicates the armor of the fighter. Armor reduces the damage taken by enemy attacks.
    /// </summary>
    public float Toughness { get; set; }

    /// <summary>
    /// Indicates the fighter's ability to regenerate. Regeneration restores life points every round.
    /// </summary>
    public float Regeneration { get; set; }

    /// <summary>
    /// Indicates the vision of the fighter. The vision affects the distance the enemy can be seen at.
    /// </summary>
    public float Vision { get; set; }

    /// <summary>
    /// Indicates the expertise. Expertise increases chance of critical hits with attacks.
    /// </summary>
    public float Expertise { get; set; }

    public IStats Clone()
    {
      return new Stats()
      {
        Accuracy = Accuracy,
        Agility = Agility,
        Expertise = Expertise,
        Power = Power,
        Regeneration = Regeneration,
        Speed = Speed,
        Stamina = Stamina,
        Toughness = Toughness,
        Vision = Vision,
        Vitality = Vitality,
      };
    }
  }
}
