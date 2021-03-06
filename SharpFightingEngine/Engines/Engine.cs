﻿using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Combat;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills.Conditions;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Engines
{
  public class Engine
  {
    private readonly EngineConfiguration configuration;

    public Engine(Func<EngineConfiguration, EngineConfiguration> configure, IEnumerable<IFighterStats> fighters)
      : this(configure.Invoke(new EngineConfiguration()), fighters)
    {
    }

    public Engine(EngineConfiguration configuration, IEnumerable<IFighterStats> fighters)
    {
      if (configuration == null)
      {
        throw new ArgumentNullException(nameof(configuration));
      }

      configuration.Validate();
      configuration.Battlefield.CurrentBounds = configuration.Bounds;
      configuration.Battlefield.NextBounds = configuration.Bounds;

      Fighters = GetDictionary(fighters);
      this.configuration = configuration;
    }

    private EngineRoundTick CurrentRoundTick { get; set; }

    private ICollection<EngineRoundTick> EngineRoundTicks { get; } = new List<EngineRoundTick>();

    private int Round { get; set; }

    private Dictionary<Guid, IFighterStats> Fighters { get; set; }

    private Dictionary<Guid, IFighterStats> DeadFighters { get; set; } = new Dictionary<Guid, IFighterStats>();

    private Dictionary<Type, Func<IFighterAction, IEnumerable<EngineTick>>> ActionHandlers => new Dictionary<Type, Func<IFighterAction, IEnumerable<EngineTick>>>()
    {
      [typeof(IMove)] = o => (o as IMove).Handle(Fighters, configuration.CalculationValues).Yield(),
      [typeof(IAttack)] = o => (o as IAttack).Handle(Fighters, EngineRoundTicks, configuration.CalculationValues),
      [typeof(IHeal)] = o => (o as IHeal).Handle(Fighters, EngineRoundTicks, configuration.CalculationValues),
    };

    public IMatchResult StartMatch()
    {
      PrepareMatch();

      bool hasWinner = false;
      bool isStale = false;

      while (!hasWinner && !isStale)
      {
        Round++;

        ProcessNewRound();

        ProcessRoundActions();

        CalculateFighterStats();

        ProcessFeatures();

        hasWinner = configuration.WinCondition.HasWinner(Fighters.Values, EngineRoundTicks, configuration.CalculationValues);
        isStale = configuration.StaleCondition.IsStale(Fighters.Values, EngineRoundTicks);
      }

      var endCondition = hasWinner ? configuration.WinCondition as IEndCondition : configuration.StaleCondition as IEndCondition;

      return endCondition.GetMatchResult(Fighters.Values.Union(DeadFighters.Values).Select(o => o.AsStruct()), EngineRoundTicks);
    }

    private void PrepareMatch()
    {
      Round = 0;

      ProcessNewRound();

      PrepareFighters();

      SpawnFighters();

      CalculateFighterStats();
    }

    private void SpawnFighters()
    {
      foreach (var fighter in Fighters.Values)
      {
        CurrentRoundTick.Ticks.Add(new FighterSpawnTick()
        {
          Fighter = fighter.AsStruct(),
        });
      }
    }

    private void CalculateFighterStats()
    {
      foreach (var fighter in Fighters.Values)
      {
        fighter.Health = fighter.HealthRemaining(configuration.CalculationValues);
      }
    }

    private void PrepareFighters()
    {
      CalculateFighterStats();

      configuration.PositionGenerator.SetFighterPositions(Fighters, configuration.Battlefield);
    }

    private void ProcessNewRound()
    {
      var roundTick = new EngineRoundTick()
      {
        Round = Round,
      };
      EngineRoundTicks.Add(roundTick);
      CurrentRoundTick = roundTick;
    }

    private void ProcessFeatures()
    {
      foreach (var feature in configuration.Features)
      {
        if (feature.NeedsUpdatedDeadFighters)
        {
          CollectDeadBodies();
        }

        CurrentRoundTick.Ticks.AddRange(feature.Apply(Fighters, DeadFighters, EngineRoundTicks, configuration));

        if (feature.NeedsUpdatedDeadFighters)
        {
          CollectDeadBodies();
        }
      }
    }

    private void ProcessRoundActions()
    {
      for (int roundAction = 0; roundAction < configuration.ActionsPerRound; roundAction++)
      {
        var actions = GetFighterActions();

        foreach (IFighterAction action in actions)
        {
          if (action.Actor.HealthRemaining(configuration.CalculationValues) <= 0)
          {
            // only process actions of alive fighters
            continue;
          }

          if (action.Actor.States.OfType<ISkillCondition>().Any(o => o.PreventsPerformingActions))
          {
            CurrentRoundTick.Ticks.Add(new FighterStunnedTick()
            {
              Fighter = action.Actor.AsStruct(),
            });

            continue;
          }

          CurrentRoundTick.Ticks.AddRange(HandleFighterAction(action).Where(o => o != null));
          CollectDeadBodies();
        }
      }
    }

    private void CollectDeadBodies()
    {
      foreach (var fighter in Fighters.Values.Where(o => !o.IsAlive(configuration.CalculationValues)))
      {
        fighter.States.Clear(); // remove all pending states

        DeadFighters.Add(fighter.Id, fighter);
        CurrentRoundTick.Ticks.Add(new EngineFighterDiedTick() { Fighter = fighter.AsStruct() });
      }

      foreach (var fighter in DeadFighters)
      {
        if (!Fighters.ContainsKey(fighter.Key))
        {
          continue;
        }

        Fighters.Remove(fighter.Key);
      }
    }

    private Dictionary<Guid, IFighterStats> GetDictionary(IEnumerable<IFighterStats> fighters)
    {
      var dict = new Dictionary<Guid, IFighterStats>();
      foreach (var fighter in fighters)
      {
        dict.Add(fighter.Id, fighter);
      }

      return dict;
    }

    private IEnumerable<IFighterAction> GetFighterActions()
    {
      var fighters = Fighters.Values.Where(o => o.IsAlive(configuration.CalculationValues));

      foreach (IFighterStats fighter in configuration.MoveOrder.Next(fighters))
      {
        yield return fighter.GetFighterAction(
          fighters.GetVisibleFightersFor(fighter, configuration.CalculationValues),
          configuration.Battlefield,
          EngineRoundTicks,
          configuration.CalculationValues);
      }
    }

    private IEnumerable<EngineTick> HandleFighterAction(IFighterAction action)
    {
      return ActionHandlers[action.GetType().GetInterfaces().First()].Invoke(action);
    }
  }
}
