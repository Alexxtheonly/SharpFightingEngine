namespace SharpFightingEngine.Fighters
{
  public struct Stats : IStats
  {
    public int Power { get; set; }

    public int ConditionPower { get; set; }

    public int Precision { get; set; }

    public int Ferocity { get; set; }

    public int Accuracy { get; set; }

    public int Agility { get; set; }

    public int Armor { get; set; }

    public int Vitality { get; set; }

    public int HealingPower { get; set; }

    public int Speed { get; set; }

    public int Vision { get; set; }

    public int Level { get; set; }

    public IStats Clone()
    {
      return new Stats()
      {
        Accuracy = Accuracy,
        Agility = Agility,
        Power = Power,
        Speed = Speed,
        Armor = Armor,
        Vision = Vision,
        Vitality = Vitality,
        ConditionPower = ConditionPower,
        Ferocity = Ferocity,
        HealingPower = HealingPower,
        Precision = Precision,
        Level = Level,
      };
    }
  }
}
