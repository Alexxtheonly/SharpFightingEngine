using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Test.Genetic.EngineCalculation;
using Xunit;

namespace SharpFightingEngine.Test.Genetic
{
  public class GeneticAlgorithmTest
  {
    [Fact(Skip = "manual test only")]
    public void ShouldTestGeneticAlgorithm()
    {
      var population = new Population(50, 100, new FighterChromosome());
      var fitness = new FighterFitness();

      RunGeneticAlgorithm(population, fitness, 50);
    }

    [Fact(Skip = "manual test only")]
    public void ShouldCalculateBestEngineCalculationValues()
    {
      var population = new Population(50, 100, new EngineCalculationChromosome());
      var fitness = new EngineCalculationFitness()
      {
        MatchCount = 25,
        SinglePowerLevelMin = 25,
        SinglePowerLevelMax = 25,
      };

      var ga = RunGeneticAlgorithm(population, fitness, 150);
      var values = new EngineCalculationValues();
      (ga.BestChromosome as EngineCalculationChromosome).ApplyTo(values);

      fitness.Evaluate(ga.BestChromosome);
    }

    private static GeneticAlgorithm RunGeneticAlgorithm(Population population, IFitness fitness, int generations)
    {
      var selection = new EliteSelection();
      var crossover = new TwoPointCrossover();
      var mutation = new ReverseSequenceMutation();

      var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
      {
        Termination = new GenerationNumberTermination(generations),
      };

      ga.Start();
      return ga;
    }
  }
}
