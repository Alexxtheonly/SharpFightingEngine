using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterMovedByAttackTick : FighterTick
  {
    public IFighter Target { get; set; }

    public IPosition Current { get; set; }

    public IPosition Next { get; set; }

    public override string ToString()
    {
      return $"{base.ToString()} moved {Target.Id} from {Current.GetVector2()} to {Current.GetVector2()}";
    }
  }
}
