using System;

namespace SharpFightingEngine.Battlefields.Plain
{
  public class PlainBattlefield : IBattlefield
  {
    public PlainBattlefield(IBounds bounds)
    {
      CurrentBounds = bounds;
    }

    public Guid Id => new Guid("DC937E88-F307-4CF0-AEF5-B468D27AED4B");

    public IBounds CurrentBounds { get; }

    public IBounds NextBounds => CurrentBounds;

    public void NextRound(int round)
    {
    }
  }
}
