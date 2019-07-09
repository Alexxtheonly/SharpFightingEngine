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
using SharpFightingEngine.WinConditions;

namespace SharpFightingEngine.Test.Utilities
{
  public static class Utility
  {
    private static readonly Random Random = new Random();

    public static Engine GetDefaultEngine(int fighterCount)
    {
      return GetDefaultEngine(fighterCount, 5, 25);
    }

    public static Engine GetDefaultEngine(int fighterCount, int minPowerlevel, int maxPowerlevel)
    {
      return GetDefaultEngine(GetFighters(fighterCount, minPowerlevel, maxPowerlevel));
    }

    public static Engine GetDefaultEngine(IEnumerable<IFighterStats> fighters)
    {
      var battlefield = new PlainBattlefield(new Small());

      var features = new List<IEngineFeature>()
      {
        new FeatureRegenerateEnergy(),
        new FeatureRegenerateHealth(),
      };

      var moveOrder = new AllRandomMoveOrder();
      var positionGenerator = new AllRandomPositionGenerator();
      var winCondition = new LastManStandingWinCondition();

      return GetEngine(fighters, battlefield, features, moveOrder, positionGenerator, winCondition, 2);
    }

    public static Engine GetDefaultTeamEngine(int teamSize, int teamCount, int minPowerlevel, int maxPowerlevel)
    {
      return GetDefaultEngine(GetTeamFighters(teamSize, teamCount, minPowerlevel, maxPowerlevel));
    }

    public static Engine GetEngine(
      IEnumerable<IFighterStats> fighters,
      IBattlefield battlefield,
      ICollection<IEngineFeature> features,
      IMoveOrder moveOrder,
      IFighterPositionGenerator positionGenerator,
      IWinCondition winCondition,
      int actionsPerRound)
    {
      return new Engine(
        cfg =>
        {
          cfg.ActionsPerRound = actionsPerRound;
          cfg.Battlefield = battlefield;
          cfg.Features = features;
          cfg.MoveOrder = moveOrder;
          cfg.PositionGenerator = positionGenerator;
          cfg.WinCondition = winCondition;

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
