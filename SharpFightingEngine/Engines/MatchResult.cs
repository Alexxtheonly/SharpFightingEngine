using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines.Ticks;

namespace SharpFightingEngine.Engines
{
  public class MatchResult : IMatchResult
  {
    public IEnumerable<EngineRoundTick> Ticks { get; set; }

    public IEnumerable<FighterMatchScore> Scores { get; set; }

    public IEnumerable<TeamMatchScore> TeamScores { get; set; }

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

          var firstPlace = Scores.FirstOrDefault();
          var secondPlace = Scores.Skip(1).FirstOrDefault();
          var thirdPlace = Scores.Skip(2).FirstOrDefault();

          yield return new FighterContribution()
          {
            FighterId = score.Id,
            HasWon = score.Id == firstPlace.Id,
            IsSecond = score.Id == secondPlace.Id,
            IsThird = score.Id == thirdPlace.Id,
            Kills = score.Kills,
            Assists = score.Assists,
            MatchParticipation = sum == 0 ? 0 : attackCount / sum,
            PercentageOfRoundsAlive = score.RoundsAlive / rounds,
          };
        }
      }
    }
  }
}
