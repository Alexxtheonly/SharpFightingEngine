using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using Newtonsoft.Json;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Test.Utilities;

namespace SharpFightingEngine.Test.Genetic
{
  public class FighterFitness : IFitness
  {
    private const int DesiredPowerlevel = 300;
    private const int OpponentPowerlevel = 400;

    public double Evaluate(IChromosome chromosome)
    {
      var fighterChromosome = chromosome as FighterChromosome;
      if (fighterChromosome == null)
      {
        return 0;
      }

      var geneticFighter = new AdvancedFighter()
      {
        Id = Guid.NewGuid()
      };
      fighterChromosome.ApplyTo(geneticFighter);

      if (geneticFighter.Stats.PowerLevel() != DesiredPowerlevel)
      {
        return (Math.Min(DesiredPowerlevel, geneticFighter.Stats.PowerLevel()) - Math.Max(DesiredPowerlevel, geneticFighter.Stats.PowerLevel())) * 1000;
      }

      var json = File.ReadAllText("../../../Data/Json/strongFighters.json");

      var winCount = 0;
      var won = false;
      var fitness = 0D;
      do
      {
        var strongFighters = JsonConvert.DeserializeObject<IEnumerable<AdvancedFighter>>(json);

        var fighters = new List<IFighterStats>()
        {
          geneticFighter,
        }.Union(strongFighters);

        Engine engine = Utility.GetDefaultEngine(fighters);
        var result = engine.StartMatch();

        won = result.Scores.First().Id == geneticFighter.Id;
        if (won)
        {
          winCount++;
        }

        fitness += GetFitness(geneticFighter, result);
      }
      while (won && winCount < 25);

      return fitness;
    }

    private static double GetFitness(IFighterStats geneticFighter, IMatchResult result)
    {
      var contribution = result.Contributions.First(o => o.FighterId == geneticFighter.Id);

      var fitness = 0D;
      if (contribution.HasWon)
      {
        fitness += 200;
      }

      fitness += contribution.KillsAndAssists * 50;
      fitness += 25 * contribution.PercentageOfRoundsAlive;
      fitness += 15 * contribution.MatchParticipation;

      return fitness;
    }
  }
}
