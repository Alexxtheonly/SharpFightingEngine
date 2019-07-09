using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterTick : EngineTick
  {
    public IFighter Fighter { get; set; }

    public override string ToString()
    {
      return $"{nameof(Fighter)} {Fighter.Id}";
    }
  }
}
