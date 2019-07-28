using System;
using System.Collections.Generic;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Battlefields.Bounds;
using SharpFightingEngine.Battlefields.Plain;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.FighterPositionGenerators;
using SharpFightingEngine.Engines.MoveOrders;
using SharpFightingEngine.Features;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Fighters.Factories;
using SharpFightingEngine.StaleConditions;
using SharpFightingEngine.WinConditions;

namespace SharpFightingEngine.Test.Utilities
{
  public static class Utility
  {
    private static readonly Random Random = new Random();

    public static Engine GetDefaultEngine(int fighterCount)
    {
      return GetDefaultEngine(fighterCount, 300);
    }

    public static Engine GetDefaultEngine(int fighterCount, int powerlevel)
    {
      return GetDefaultEngine(FighterFactory.GetFighters(fighterCount, powerlevel));
    }

    public static Engine GetDefaultEngine(IEnumerable<IFighterStats> fighters)
    {
      var battlefield = new PlainBattlefield();
      var bounds = new Small();

      var features = new List<IEngineFeature>()
      {
        new FeatureRegenerateEnergy(),
        new FeatureRegenerateHealth(),
      };

      var moveOrder = new AllRandomMoveOrder();
      var positionGenerator = new AllRandomPositionGenerator();
      var winCondition = new LastManStandingWinCondition();
      var staleCondition = new NoWinnerCanBeDeterminedStaleCondition();

      return GetEngine(fighters, battlefield, bounds, features, moveOrder, positionGenerator, winCondition, staleCondition, 2);
    }

    public static Engine GetDefaultTeamEngine(int teamSize, int teamCount, int minPowerlevel, int maxPowerlevel)
    {
      return GetDefaultEngine(GetTeamFighters(teamSize, teamCount, minPowerlevel, maxPowerlevel));
    }

    public static Engine GetEngine(
      IEnumerable<IFighterStats> fighters,
      IBattlefield battlefield,
      IBounds bounds,
      ICollection<IEngineFeature> features,
      IMoveOrder moveOrder,
      IFighterPositionGenerator positionGenerator,
      IWinCondition winCondition,
      IStaleCondition staleCondition,
      int actionsPerRound)
    {
      return new Engine(
        cfg =>
        {
          cfg.ActionsPerRound = actionsPerRound;
          cfg.Battlefield = battlefield;
          cfg.Bounds = bounds;
          cfg.Features = features;
          cfg.MoveOrder = moveOrder;
          cfg.PositionGenerator = positionGenerator;
          cfg.WinCondition = winCondition;
          cfg.StaleCondition = staleCondition;

          return cfg;
        }, fighters);
    }

    private static IEnumerable<IFighterStats> GetFighters(int count, int minPowerlevel, int maxPowerlevel)
    {
      for (int i = 0; i < count; i++)
      {
        yield return GetFighter(null, minPowerlevel, maxPowerlevel);
      }
    }

    private static IEnumerable<IFighterStats> GetTeamFighters(int teamSize, int teamCount, int minPowerlevel, int maxPowerlevel)
    {
      Guid team = Guid.Empty;
      for (int i = 0; i < teamSize * teamCount; i++)
      {
        if (i % teamSize == 0)
        {
          team = Guid.NewGuid();
        }

        yield return GetFighter(team, minPowerlevel, maxPowerlevel);
      }
    }

    private static IFighterStats GetFighter(Guid? team, int minPowerlevel, int maxPowerlevel)
    {
      return new GenericFighter()
      {
        Id = Guid.NewGuid(),
        Team = team,
        Accuracy = GetRandomValue(minPowerlevel, maxPowerlevel),
        Agility = GetRandomValue(minPowerlevel, maxPowerlevel),
        Toughness = GetRandomValue(minPowerlevel, maxPowerlevel),
        Power = GetRandomValue(minPowerlevel, maxPowerlevel),
        Regeneration = GetRandomValue(minPowerlevel, maxPowerlevel),
        Speed = GetRandomValue(minPowerlevel, maxPowerlevel),
        Stamina = GetRandomValue(minPowerlevel, maxPowerlevel),
        Vision = GetRandomValue(minPowerlevel, maxPowerlevel),
        Vitality = GetRandomValue(minPowerlevel, maxPowerlevel),
        Expertise = GetRandomValue(minPowerlevel, maxPowerlevel),
      };
    }

    private static int GetRandomValue(int min, int max)
    {
      return Random.Next(min, max);
    }
  }
}
