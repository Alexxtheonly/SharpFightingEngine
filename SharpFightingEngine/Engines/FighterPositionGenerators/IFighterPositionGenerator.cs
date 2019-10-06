using System;
using System.Collections.Generic;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Engines.FighterPositionGenerators
{
  public interface IFighterPositionGenerator
  {
    Guid Id { get; }

    void SetFighterPosition(IFighterStats fighter, IBattlefield battlefield);

    void SetFighterPositions(Dictionary<Guid, IFighterStats> fighters, IBattlefield battlefield);
  }
}
