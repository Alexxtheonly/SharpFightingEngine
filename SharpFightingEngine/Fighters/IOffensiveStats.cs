namespace SharpFightingEngine.Fighters
{
  public interface IOffensiveStats
  {
    int Power { get; set; }

    int ConditionPower { get; set; }

    int Precision { get; set; }

    int Ferocity { get; set; }

    int Accuracy { get; set; }
  }
}
