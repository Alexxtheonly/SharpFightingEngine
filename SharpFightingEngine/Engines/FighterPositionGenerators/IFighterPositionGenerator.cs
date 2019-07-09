using System;
using System.Collections.Generic;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Engines.FighterPositionGenerators
{
  public interface IFighterPositionGenerator
  {
    void SetFighterPositions(Dictionary<Guid, IFighterStats> fighters, IBattlefield battlefield);
  }
}
