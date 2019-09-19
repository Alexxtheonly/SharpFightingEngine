namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterStunnedTick : FighterTick
  {
    public override string ToString()
    {
      return $"{base.ToString()} is stunned.";
    }
  }
}
