using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Combat
{
  public class Move : IMove
  {
    public IPosition NextPosition { get; set; }

    public IFighterStats Actor { get; set; }
  }
}
