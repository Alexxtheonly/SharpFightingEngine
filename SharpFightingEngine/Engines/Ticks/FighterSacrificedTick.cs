namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterSacrificedTick : FighterTick
  {
    public override string ToString()
    {
      return $"{base.ToString()} has been sacrificed.";
    }
  }
}
