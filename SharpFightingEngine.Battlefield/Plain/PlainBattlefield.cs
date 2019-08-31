using System;
using SharpFightingEngine.Battlefields.Constants;

namespace SharpFightingEngine.Battlefields.Plain
{
  public class PlainBattlefield : IBattlefield
  {
    public Guid Id => BattlefieldConstants.Plain;

    public IBounds CurrentBounds { get; set; }

    public IBounds NextBounds { get; set; }
  }
}
