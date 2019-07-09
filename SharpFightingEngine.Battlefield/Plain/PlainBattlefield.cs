namespace SharpFightingEngine.Battlefields.Plain
{
  public class PlainBattlefield : IBattlefield
  {
    public PlainBattlefield(IBounds bounds)
    {
      CurrentBounds = bounds;
    }

    public IBounds CurrentBounds { get; }

    public IBounds NextBounds => CurrentBounds;

    public void NextRound(int round)
    {
    }
  }
}
