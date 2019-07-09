namespace SharpFightingEngine.Engines.Ticks
{
  public class EngineFighterDiedTick : FighterTick
  {
    public override string ToString()
    {
      return $"{base.ToString()} has been killed";
    }
  }
}
