using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Engines
{
  public class MatchResult : IMatchResult
  {
    public IEnumerable<EngineRoundTick> Ticks { get; set; }

    public IEnumerable<FighterMatchScore> Scores => Ticks
      .CalculateFighterMatchScores()
      .OrderScores();

    public IEnumerable<TeamMatchScore> TeamScores => Scores
      .Where(o => o.TeamId != null)
      .GroupBy(o => o.TeamId)
      .Select(o => new TeamMatchScore()
      {
        Id = o.Key ?? default,
        RoundsAlive = o.Max(x => x.RoundsAlive),
        TotalDamageDone = o.Sum(x => x.TotalDamageDone),
        TotalDamageTaken = o.Sum(x => x.TotalDamageTaken),
        TotalDeaths = o.Sum(x => x.TotalDeaths),
        TotalDistanceTraveled = o.Sum(x => x.TotalDistanceTraveled),
        TotalEnergyUsed = o.Sum(x => x.TotalEnergyUsed),
        TotalKills = o.Sum(x => x.TotalKills),
        TotalRegeneratedEnergy = o.Sum(x => x.TotalRegeneratedEnergy),
        TotalRegeneratedHealth = o.Sum(x => x.TotalRegeneratedHealth),
      })
      .OrderScores();

    public IEnumerable<FighterContribution> Contributions
    {
      get
      {
        double rounds = Ticks.GetLastRound().Round;

        foreach (var score in Scores)
        {
          var attackCount = Ticks.SelectMany(o => o.Ticks).OfType<FighterAttackTick>().Where(o => o.Fighter.Id == score.Id).Count();
          var moveCount = Ticks.SelectMany(o => o.Ticks).OfType<FighterMoveTick>().Where(o => o.Fighter.Id == score.Id).Count();
          double sum = attackCount + moveCount;

          yield return new FighterContribution()
          {
            FighterId = score.Id,
            HasWon = score.Id == Scores.First().Id,
            KillsAndAssists = score.TotalKills,
            MatchParticipation = sum == 0 ? 0 : attackCount / sum,
            PercentageOfRoundsAlive = score.RoundsAlive / rounds,
          };
        }
      }
    }
  }
}
