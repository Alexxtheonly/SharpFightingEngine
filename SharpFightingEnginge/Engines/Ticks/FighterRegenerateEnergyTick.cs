namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterRegenerateEnergyTick : FighterTick
  {
    public int RegeneratedEnergy { get; set; }

    public override string ToString()
    {
      return $"\t{Fighter.Id} regenerated {RegeneratedEnergy} energy";
    }
  }
}
