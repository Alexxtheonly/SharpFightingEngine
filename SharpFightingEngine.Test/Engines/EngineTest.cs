using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Battlefields.Bounds;
using SharpFightingEngine.Battlefields.Plain;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.FighterPositionGenerators;
using SharpFightingEngine.Engines.MoveOrders;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Features;
using SharpFightingEngine.Fighters;
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
      var engine = Utility.GetDefaultEngine(20, 500);
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
    public void ShouldRegenerateCorrectEnergy()
    {
      var json = File.ReadAllText(@"../../../Data/Json/league300-20190906.json");

      var fighters = JsonConvert.DeserializeObject<IEnumerable<AdvancedFighter>>(json);

      var engine = Utility.GetDefaultEngine(fighters);
      var result = engine.StartMatch();
    }

    [Fact]
    public void ShouldHaveFighterSacrificedTick()
    {
      var json = File.ReadAllText(@"../../../Data/Json/fightersWithTroll.json");

      var fighters = JsonConvert.DeserializeObject<IEnumerable<AdvancedFighter>>(json);

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
        .OfType<FighterSacrificedTick>();

      Assert.NotEmpty(ticks);
      var score = result.Scores.FirstOrDefault(o => o.Id == ticks.First().Fighter.Id);
      Assert.True(score.RoundsAlive == 1);
      Assert.True(score.TotalDeaths == 1);
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

      foreach (var contribution in result.Contributions)
      {
        var score = result.Scores.First(o => o.Id == contribution.FighterId);

        Assert.Equal(score.TotalKills, contribution.KillsAndAssists);
        Assert.Equal(result.Scores.First().Id == contribution.FighterId, contribution.HasWon);
      }

      var contributedKills = result.Contributions.Sum(o => o.KillsAndAssists);
      Assert.True(contributedKills >= result.Scores.Count());
      Assert.NotEqual(0, result.Contributions.Sum(o => o.MatchParticipation));
      Assert.NotEqual(0, result.Contributions.Sum(o => o.PercentageOfRoundsAlive));
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
