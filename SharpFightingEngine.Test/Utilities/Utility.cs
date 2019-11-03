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
    public static void SetStats(IStats stats, int level, int weaponPlus, int armorPlus, int weaponRarity, int armorRarity)
    {
      const int baseValue = 10;

      stats.Power = baseValue * (level + weaponPlus + weaponRarity);
      stats.Armor = baseValue * (level + armorPlus + armorRarity);
      stats.Accuracy = baseValue * level;
      stats.Agility = baseValue * level;
      stats.ConditionPower = baseValue * level;
      stats.Ferocity = 50;
      stats.HealingPower = baseValue * level;
      stats.Level = level;
      stats.Precision = 20;
      stats.Speed = 10;
      stats.Vision = 10;
      stats.Vitality = baseValue * level;
      stats.ParryChance = 20;
    }

    public static Engine GetDefaultEngine(int fighterCount)
    {
      return GetDefaultEngine(fighterCount, 300);
    }

    public static Engine GetDefaultEngine(int fighterCount, Action<IStats> customStats)
    {
      return GetDefaultEngine(FighterFactory.GetFighters(fighterCount, customStats));
    }

    public static Engine GetDefaultDeathmatchEngine(int fighterCount, Action<IStats> customStats)
    {
      return GetDefaultDeathmatchEngine(FighterFactory.GetFighters(fighterCount, customStats));
    }

    public static Engine GetDefaultEngine(int fighterCount, int powerlevel)
    {
      return GetDefaultEngine(FighterFactory.GetFighters(fighterCount, powerlevel));
    }

    public static Engine GetDefaultEngine(IEnumerable<IFighterStats> fighters)
    {
      var battlefield = new PlainBattlefield();
      var bounds = new Tiny();

      var features = new List<IEngineFeature>()
      {
        new FeatureApplyCondition(),
        new FeatureSacrificeToEntity(),
      };

      var moveOrder = new AllRandomMoveOrder();
      var positionGenerator = new AllRandomPositionGenerator();
      var winCondition = new LastManStandingWinCondition();
      var staleCondition = new NoWinnerCanBeDeterminedStaleCondition();

      return GetEngine(fighters, battlefield, bounds, features, moveOrder, positionGenerator, winCondition, staleCondition, 2);
    }

    public static Engine GetDefaultDeathmatchEngine(IEnumerable<IFighterStats> fighters)
    {
      var battlefield = new PlainBattlefield();
      var bounds = new Tiny();

      var features = new List<IEngineFeature>()
      {
        new FeatureApplyCondition(),
        new FeatureReviveDeadFighters(),
      };

      var moveOrder = new AllRandomMoveOrder();
      var positionGenerator = new AllRandomPositionGenerator();
      var winCondition = new FiftyRoundsWinCondition();
      var staleCondition = new NoneStaleCondition();

      return GetEngine(fighters, battlefield, bounds, features, moveOrder, positionGenerator, winCondition, staleCondition, 2);
    }

    public static Engine GetDefaultTeamEngine(int teamSize, int teamCount, int powerlevel)
    {
      return GetDefaultEngine(GetTeamFighters(teamSize, teamCount, powerlevel));
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

    private static IEnumerable<IFighterStats> GetTeamFighters(int teamSize, int teamCount, int powerlevel)
    {
      Guid team = Guid.Empty;
      for (int i = 0; i < teamSize * teamCount; i++)
      {
        if (i % teamSize == 0)
        {
          team = Guid.NewGuid();
        }

        yield return FighterFactory.GetFighter(powerlevel, team);
      }
    }
  }
}
