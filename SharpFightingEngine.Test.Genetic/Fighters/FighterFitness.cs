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

      if (geneticFighter.PowerLevel() != DesiredPowerlevel)
      {
        return (Math.Min(DesiredPowerlevel, geneticFighter.PowerLevel()) - Math.Max(DesiredPowerlevel, geneticFighter.PowerLevel())) * 1000;
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

        won = result.Wins.Any(o => o.Id == geneticFighter.Id);
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
      var fitness = 0D;
      var score = result.Scores.FirstOrDefault(o => o.Id == geneticFighter.Id);

      fitness += score.TotalKills * 25;
      fitness += score.TotalDamageDone * 0.2;

      if (result.Loses.Any(o => o.Id == geneticFighter.Id))
      {
        fitness -= (result.Loses.TakeWhile(o => o.Id != geneticFighter.Id).Count() + 1) * 5;

        return fitness;
      }

      if (result.Draws.Any(o => o.Id == geneticFighter.Id))
      {
        fitness += result.Loses.TakeWhile(o => o.Id != geneticFighter.Id).Count() + 1;

        return fitness;
      }

      return fitness + 2500;
    }
  }
}
