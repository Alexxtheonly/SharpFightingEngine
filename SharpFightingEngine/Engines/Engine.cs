using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Combat;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

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

    private bool TeamMode { get; set; }

    private Dictionary<Guid, IFighterStats> Fighters { get; set; }

    private Dictionary<Guid, IFighterStats> DeadFighters { get; set; } = new Dictionary<Guid, IFighterStats>();

    private Dictionary<Type, Func<IFighterAction, EngineTick>> ActionHandlers => new Dictionary<Type, Func<IFighterAction, EngineTick>>()
    {
      [typeof(IMove)] = o => (o as IMove).Handle(Fighters),
      [typeof(IAttack)] = o => (o as IAttack).Handle(Fighters, configuration.CalculationValues),
    };

    public IMatchResult StartMatch()
    {
      PrepareMatch();

      while (!configuration.WinCondition.HasWinner(Fighters.Values) && !configuration.StaleCondition.IsStale(Fighters.Values, EngineRoundTicks))
      {
        Round++;

        ProcessNewRound();

        ProcessRoundActions();

        CalculateFighterStats();

        CalculateRoundScore();

        CollectDeadBodies();

        ProcessFeatures();
      }

      return new MatchResult()
      {
        Ticks = EngineRoundTicks,
      };
    }

    private void PrepareMatch()
    {
      Round = 0;

      // if any fighter has a team we consider it team mode
      TeamMode = Fighters.Values.Any(o => o.Team != null);

      ProcessNewRound();

      PrepareFighters();

      SpawnFighters();

      CalculateFighterStats();

      CalculateRoundScore();
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
        fighter.Health = fighter.Health(configuration.CalculationValues);
        fighter.Energy = fighter.Energy(configuration.CalculationValues);
      }
    }

    private void PrepareFighters()
    {
      CalculateFighterStats();

      configuration.PositionGenerator.SetFighterPositions(Fighters, configuration.Battlefield);
    }

    private void CalculateRoundScore()
    {
      foreach (var fighter in Fighters.Values)
      {
        CurrentRoundTick.ScoreTick.Add(new EngineRoundScoreTick(CurrentRoundTick, fighter.AsStruct()));
      }

      if (TeamMode)
      {
        foreach (var team in Fighters.Values
          .Where(o => o.Team != null)
          .Select(o => o.AsStruct())
          .GroupBy(o => o.Team))
        {
          CurrentRoundTick.ScoreTick.Add(new EngineRoundTeamScoreTick(CurrentRoundTick, team));
        }
      }
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
        CurrentRoundTick.Ticks.AddRange(feature.Apply(Fighters.Values, configuration.CalculationValues));
      }
    }

    private void ProcessRoundActions()
    {
      for (int roundAction = 0; roundAction < configuration.ActionsPerRound; roundAction++)
      {
        var actions = GetFighterActions();

        foreach (IFighterAction action in actions)
        {
          if (action.Actor.Health(configuration.CalculationValues) <= 0)
          {
            // only process actions of alive fighters
            continue;
          }

          CurrentRoundTick.Ticks.Add(HandleFighterAction(action));
        }
      }
    }

    private void CollectDeadBodies()
    {
      foreach (var fighter in Fighters.Values.Where(o => !o.IsAlive()))
      {
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
      var fighters = Fighters.Values.Where(o => o.IsAlive());

      foreach (IFighterStats fighter in fighters)
      {
        yield return fighter.GetFighterAction(fighters.GetVisibleFightersFor(fighter, configuration.CalculationValues), configuration.Battlefield);
      }
    }

    private EngineTick HandleFighterAction(IFighterAction action)
    {
      return ActionHandlers[action.GetType().GetInterfaces().First()].Invoke(action);
    }
  }
}
