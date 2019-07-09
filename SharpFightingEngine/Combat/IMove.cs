using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Combat
{
  public interface IMove : IFighterAction
  {
    IPosition NextPosition { get; }
  }
}
