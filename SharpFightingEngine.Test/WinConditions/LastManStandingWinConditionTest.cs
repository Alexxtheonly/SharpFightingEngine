using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Fighters.Factories;
using SharpFightingEngine.WinConditions;
using Xunit;

namespace SharpFightingEngine.Test.WinConditions
{
  public class LastManStandingWinConditionTest
  {
    private readonly LastManStandingWinCondition winCondition = new LastManStandingWinCondition();
    private readonly EngineCalculationValues values = new EngineCalculationValues();

    private readonly IEnumerable<IFighterStats> fightersNoTeam = FighterFactory.GetFighters(4, 300).ToList();
    private readonly IEnumerable<IFighterStats> fightersTeam = FighterFactory.GetFighterTeams(4, 3, 300).ToList();

    public LastManStandingWinConditionTest()
    {
      CalculateFighterHealth(ref fightersNoTeam);
      CalculateFighterHealth(ref fightersTeam);
    }

    [Fact]
    public void ShouldHaveWinnerNoTeams()
    {
      foreach (var fighter in fightersNoTeam.Skip(1))
      {
        fighter.Health = 0;
      }

      Assert.True(winCondition.HasWinner(fightersNoTeam));
    }

    [Fact]
    public void ShouldNotHaveWinnerNoTeams()
    {
      Assert.False(winCondition.HasWinner(fightersNoTeam));
    }

    [Fact]
    public void ShouldHaveWinnerTeams()
    {
      foreach (var team in fightersTeam.GroupBy(o => o.Team).Skip(1))
      {
        foreach (var fighter in team)
        {
          fighter.Health = 0;
        }
      }

      Assert.True(winCondition.HasWinner(fightersTeam));
    }

    [Fact]
    public void ShouldNotHaveWinnerTeams()
    {
      Assert.False(winCondition.HasWinner(fightersTeam));
    }

    private void CalculateFighterHealth(ref IEnumerable<IFighterStats> fighters)
    {
      foreach (var fighter in fighters)
      {
        fighter.Health = fighter.Health(values);
      }
    }
  }
}
