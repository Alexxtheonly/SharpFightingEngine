using System;

namespace SharpFightingEngine.Battlefields.Plain
{
  public class PlainBattlefield : IBattlefield
  {
    public PlainBattlefield()
    {
    }

    public Guid Id => new Guid("DC937E88-F307-4CF0-AEF5-B468D27AED4B");

    public IBounds CurrentBounds { get; set; }

    public IBounds NextBounds { get; set; }
  }
}
