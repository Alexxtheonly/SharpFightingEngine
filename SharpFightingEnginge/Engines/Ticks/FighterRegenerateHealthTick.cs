namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterRegenerateHealthTick : FighterTick
  {
    public int HealthPointsRegenerated { get; set; }

    public override string ToString()
    {
      return $"\t{Fighter.Id} regenerated {HealthPointsRegenerated} health points";
    }
  }
}
