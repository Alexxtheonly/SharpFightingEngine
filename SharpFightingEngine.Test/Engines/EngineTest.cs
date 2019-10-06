using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Test.Data.Engines;
using SharpFightingEngine.Test.Utilities;
using Xunit;

namespace SharpFightingEngine.Test.Engines
{
  public class EngineTest
  {
    [Theory]
    [InlineData(1, 0, 0, 0, 0)]
    [InlineData(1, 5, 0, 0, 0)]
    [InlineData(1, 7, 0, 5, 0)]
    [InlineData(8, 0, 0, 0, 0)]
    [InlineData(16, 0, 0, 0, 0)]
    [InlineData(24, 0, 0, 0, 0)]
    [InlineData(32, 7, 5, 5, 4)]
    public void ShouldReturnMatchResultFor(int level, int weaponPlus, int weaponRarity, int armorPlus, int armorRarity)
    {
      var engine = Utility.GetDefaultEngine(20, stats =>
      {
        Utility.SetStats(stats, level, weaponPlus, armorPlus, weaponRarity, armorRarity);
      });

      var result = engine.StartMatch();

      Assert.NotNull(result);
      Assert.NotEmpty(result.Ticks);
      Assert.NotEmpty(result.Scores);
      VerifyMatchResult(result);
    }

    [Theory]
    [InlineData(1, 0, 0, 0, 0)]
    [InlineData(1, 5, 0, 0, 0)]
    [InlineData(1, 7, 0, 5, 0)]
    [InlineData(8, 0, 0, 0, 0)]
    [InlineData(16, 0, 0, 0, 0)]
    [InlineData(24, 0, 0, 0, 0)]
    [InlineData(32, 7, 5, 5, 4)]
    public void ShouldReturnDeathmatchMatchResults(int level, int weaponPlus, int weaponRarity, int armorPlus, int armorRarity)
    {
      var engine = Utility.GetDefaultDeathmatchEngine(3, stats =>
      {
        Utility.SetStats(stats, level, weaponPlus, armorPlus, weaponRarity, armorRarity);
      });

      var result = engine.StartMatch();

      Assert.NotNull(result);
      Assert.NotEmpty(result.Ticks);
      Assert.NotEmpty(result.Scores);
      VerifyMatchResult(result);
    }

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
      Assert.True(contributedKills >= result.Scores.Count() - 1);
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
    }
  }
}
