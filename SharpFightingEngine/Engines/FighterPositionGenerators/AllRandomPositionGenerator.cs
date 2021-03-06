﻿using System;
using System.Collections.Generic;
using System.Numerics;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Constants;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Engines.FighterPositionGenerators
{
  public class AllRandomPositionGenerator : IFighterPositionGenerator
  {
    private readonly Random random = new Random();

    public Guid Id => FighterPositionGeneratorConstants.AllRandom;

    public void SetFighterPosition(IFighterStats fighter, IBattlefield battlefield)
    {
      fighter.Set(new Vector3()
      {
        X = random.Next((int)battlefield.CurrentBounds.Low.X, (int)battlefield.CurrentBounds.High.X),
        Y = random.Next((int)battlefield.CurrentBounds.Low.Y, (int)battlefield.CurrentBounds.High.Y),
        Z = random.Next((int)battlefield.CurrentBounds.Low.Z, (int)battlefield.CurrentBounds.High.Z),
      });
    }

    public void SetFighterPositions(Dictionary<Guid, IFighterStats> fighters, IBattlefield battlefield)
    {
      foreach (var fighter in fighters.Values)
      {
        SetFighterPosition(fighter, battlefield);
      }
    }
  }
}
