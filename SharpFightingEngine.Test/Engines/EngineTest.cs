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
      var engine = Utility.GetDefaultTeamEngine(4, 5, 5, 25);
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

    private void VerifyMatchScore(IEnumerable<FighterTick> allFighterTicks, MatchScore score, Func<FighterTick, bool> actorQuery, Func<FighterAttackTick, bool> targetQuery)
    {
      var fighterTicks = allFighterTicks
        .Where(actorQuery)
        .ToList();

      Assert.Equal(fighterTicks.OfType<FighterMoveTick>().Sum(o => o.Current.GetDistance(o.Next)), score.TotalDistanceTraveled, 3);
      Assert.Equal(fighterTicks.OfType<FighterAttackTick>().Where(o => o.Hit).Sum(o => o.Damage), score.TotalDamageDone);
      Assert.Equal(allFighterTicks.OfType<FighterAttackTick>().Where(targetQuery).Where(o => o.Hit).Sum(o => o.Damage), score.TotalDamageTaken);
      Assert.Equal(fighterTicks.OfType<EngineFighterDiedTick>().Count(), score.TotalDeaths);
      Assert.Equal(fighterTicks.OfType<FighterAttackTick>().Sum(o => o.Skill.Energy), score.TotalEnergyUsed);
    }
  }
}
