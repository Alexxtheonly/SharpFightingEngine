using System;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
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

      var geneticFighter = new GenericFighter()
      {
        Id = Guid.NewGuid()
      };
      fighterChromosome.ApplyTo(geneticFighter);

      if (geneticFighter.PowerLevel() != DesiredPowerlevel)
      {
        return (Math.Min(DesiredPowerlevel, geneticFighter.PowerLevel()) - Math.Max(DesiredPowerlevel, geneticFighter.PowerLevel())) * 1000;
      }

      // FighterFactory.GetFighters(9, OpponentPowerlevel)
      var fighters = new List<IFighterStats>()
      {
        geneticFighter,
        new GenericFighter()
        {
          Id = Guid.NewGuid(),
          Power = 142,
          Accuracy = 3,
          Expertise = 3,
          Agility = 2,
          Toughness = 61,
          Vitality = 51,
          Speed = 2,
          Stamina = 20,
          Regeneration = 3,
          Vision = 13,
        }
      };

      Engine engine = Utility.GetDefaultEngine(fighters);

      double score = 0;
      for (int i = 0; i < 5; i++)
      {
        score += GetFitness(geneticFighter, engine.StartMatch());
      }

      return score;
    }

    private static double GetFitness(GenericFighter geneticFighter, IMatchResult result)
    {
      if (result.Loses.Any(o => o.Id == geneticFighter.Id))
      {
        return -(result.Loses.TakeWhile(o => o.Id != geneticFighter.Id).Count() + 1) * 5;
      }

      if (result.Draws.Any(o => o.Id == geneticFighter.Id))
      {
        return result.Loses.TakeWhile(o => o.Id != geneticFighter.Id).Count() + 1;
      }

      var score = result.Scores.FirstOrDefault(o => o.Id == geneticFighter.Id);

      return 2500 + ((score.TotalDamageDone - score.TotalDamageTaken) * 5);
    }
  }
}
