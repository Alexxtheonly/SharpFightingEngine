namespace SharpFightingEngine.Battlefields
{
  public interface IBattlefield
  {
    IBounds CurrentBounds { get; }

    IBounds NextBounds { get; }

    void NextRound(int round);
  }
}
