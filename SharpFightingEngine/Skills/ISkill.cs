using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills
{
  public interface ISkill
  {
    Guid Id { get; }

    string Name { get; }

    float Range { get; }

    int Cooldown { get; }

    IEnumerable<EngineTick> Perform(IFighterStats actor, IFighterStats target, EngineCalculationValues calculationValues);
  }
}
