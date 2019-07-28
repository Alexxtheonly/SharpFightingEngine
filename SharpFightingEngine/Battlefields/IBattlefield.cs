using System;

namespace SharpFightingEngine.Battlefields
{
  public interface IBattlefield
  {
    Guid Id { get; }

    IBounds CurrentBounds { get; set; }

    IBounds NextBounds { get; set; }
  }
}
