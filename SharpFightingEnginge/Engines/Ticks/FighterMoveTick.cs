using SharpFightingEngine.Battlefields;

namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterMoveTick : FighterTick
  {
    public IPosition Current { get; set; }

    public IPosition Next { get; set; }

    public override string ToString()
    {
      return $"{base.ToString()} move from {Current.GetVector3()} to {Next.GetVector3()}";
    }
  }
}
