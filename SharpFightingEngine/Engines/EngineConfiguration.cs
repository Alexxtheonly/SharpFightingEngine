using System;
using System.Collections.Generic;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines.FighterPositionGenerators;
using SharpFightingEngine.Engines.MoveOrders;
using SharpFightingEngine.Features;
using SharpFightingEngine.StaleConditions;
using SharpFightingEngine.WinConditions;

namespace SharpFightingEngine.Engines
{
  public class EngineConfiguration
  {
    public IBattlefield Battlefield { get; set; }

    public int ActionsPerRound { get; set; }

    public IMoveOrder MoveOrder { get; set; }

    public ICollection<IEngineFeature> Features { get; set; } = new List<IEngineFeature>();

    public IWinCondition WinCondition { get; set; }

    public IStaleCondition StaleCondition { get; set; }

    public IFighterPositionGenerator PositionGenerator { get; set; }

    public EngineCalculationValues CalculationValues { get; private set; } = new EngineCalculationValues();

    public void Validate()
    {
      if (Battlefield == null)
      {
        throw new ArgumentNullException(nameof(Battlefield));
      }

      if (ActionsPerRound == 0)
      {
        throw new ArgumentException($"At least one action per round must be performed.");
      }

      if (MoveOrder == null)
      {
        throw new ArgumentNullException(nameof(MoveOrder));
      }

      if (WinCondition == null)
      {
        throw new ArgumentNullException(nameof(WinCondition));
      }

      if (StaleCondition == null)
      {
        throw new ArgumentNullException(nameof(StaleCondition));
      }

      if (PositionGenerator == null)
      {
        throw new ArgumentNullException(nameof(PositionGenerator));
      }
    }
  }
}
