using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Battlefields.Bounds;
using SharpFightingEngine.Battlefields.Plain;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.FighterPositionGenerators;
using SharpFightingEngine.Engines.MoveOrders;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Features;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Fighters.Factories;
using SharpFightingEngine.StaleConditions;
using SharpFightingEngine.Test.Data.Engines;
using SharpFightingEngine.Test.Utilities;
using SharpFightingEngine.WinConditions;
using Xunit;

namespace SharpFightingEngine.Test.Engines
{
  public class EngineTest
  {
    [Theory]
    [ClassData(typeof(AllRandomGenericFighterTheoryData))]
    [ClassData(typeof(AllRandomGenericFighterTheoryData))]
    [ClassData(typeof(AllRandomGenericFighterTheoryData))]
    [ClassData(typeof(AllRandomGenericFighterTheoryData))]
    public void ShouldReturnMatchResult(Engine engine)
    {
      var result = engine.StartMatch();

      Assert.NotNull(result);
      Assert.NotEmpty(result.Ticks);
      Assert.NotEmpty(result.Scores);
      VerifyMatchResult(result);
    }

    [Fact]
    public void TeamMatchShouldReturnMatchResult()
    {
      var engine = Utility.GetDefaultTeamEngine(4, 5, 300);
      var result = engine.StartMatch();

      VerifyMatchResult(result);
    }

    [Fact]
    public void ShouldHaveRegenerateHealthTicks()
    {
      var engine = Utility.GetDefaultEngine(20);
      var result = engine.StartMatch();

      Assert.NotEmpty(result.Ticks.SelectMany(o => o.Ticks).OfType<FighterRegenerateHealthTick>());
    }

    [Fact]
    public void ShouldHaveRegenerateEnergyTicks()
    {
      var engine = Utility.GetDefaultEngine(20);
      var result = engine.StartMatch();

      Assert.NotEmpty(result.Ticks.SelectMany(o => o.Ticks).OfType<FighterRegenerateEnergyTick>());
    }

    [Fact]
    public void ShouldHaveFighterSacrificedTick()
    {
      var sacrificeFighter = new AdvancedFighter()
      {
        Id = Guid.NewGuid(),
        Accuracy = 1,
        Agility = 291,
        Expertise = 1,
        Power = 1,
        Regeneration = 1,
        Speed = 1,
        Stamina = 1,
        Toughness = 1,
        Vision = 1,
        Vitality = 1,
      };

      var fighters = FighterFactory.GetFighters(19, 300)
        .Union(new AdvancedFighter[]
        {
          sacrificeFighter,
        });
      var battlefield = new PlainBattlefield();
      var features = new List<IEngineFeature>()
      {
        new FeatureRegenerateEnergy(),
        new FeatureRegenerateHealth(),
        new FeatureSacrificeToEntity(),
      };

      var engine = Utility.GetEngine(
        fighters,
        battlefield,
        new Small(),
        features,
        new AllRandomMoveOrder(),
        new AllRandomPositionGenerator(),
        new LastManStandingWinCondition(),
        new NoWinnerCanBeDeterminedStaleCondition(),
        2);

      var result = engine.StartMatch();
      var ticks = result.Ticks
        .SelectMany(o => o.Ticks)
        .OfType<FighterSacrificedTick>()
        .ToList();

      Assert.Contains(ticks, o => o.Fighter.Id == sacrificeFighter.Id);
    }

    private void VerifyMatchResult(IMatchResult result)
    {
      var allFighterTicks = result.Ticks
              .SelectMany(o => o.Ticks)
              .OfType<FighterTick>();

      Assert.NotEmpty(allFighterTicks);

      foreach (var score in result.Scores)
      {
        VerifyMatchScore(allFighterTicks, score, o => o.Fighter.Id == score.Id, o => o.Target.Id == score.Id);
      }

      foreach (var teamScore in result.TeamScores)
      {
        VerifyMatchScore(allFighterTicks, teamScore, o => o.Fighter.Team == teamScore.Id, o => o.Target.Team == teamScore.Id);
      }
    }

    private void VerifyMatchScore(IEnumerable<FighterTick> allFighterTicks, IMatchScore score, Func<FighterTick, bool> actorQuery, Func<FighterAttackTick, bool> targetQuery)
    {
      var fighterTicks = allFighterTicks
        .Where(actorQuery)
        .ToList();

      Assert.Equal(fighterTicks.OfType<FighterMoveTick>().Sum(o => o.Current.GetDistance(o.Next)), score.TotalDistanceTraveled, 2);
      Assert.Equal(fighterTicks.OfType<FighterAttackTick>().Where(o => o.Hit).Sum(o => o.Damage), score.TotalDamageDone);
      Assert.Equal(allFighterTicks.OfType<FighterAttackTick>().Where(targetQuery).Where(o => o.Hit).Sum(o => o.Damage), score.TotalDamageTaken);
      Assert.Equal(fighterTicks.OfType<EngineFighterDiedTick>().Count(), score.TotalDeaths);
      Assert.Equal(fighterTicks.OfType<FighterAttackTick>().Sum(o => o.Skill.Energy), score.TotalEnergyUsed);
    }
  }
}
