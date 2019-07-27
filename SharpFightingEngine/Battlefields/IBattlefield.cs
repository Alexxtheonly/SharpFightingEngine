using System;

namespace SharpFightingEngine.Battlefields
{
  public interface IBattlefield
  {
    Guid Id { get; }

    IBounds CurrentBounds { get; }

    IBounds NextBounds { get; }

    void NextRound(int round);
  }
}
