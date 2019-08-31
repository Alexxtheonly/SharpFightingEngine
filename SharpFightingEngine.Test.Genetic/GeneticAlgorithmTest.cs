using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Test.Genetic.EngineCalculation;
using Xunit;
using Xunit.Abstractions;

namespace SharpFightingEngine.Test.Genetic
{
  public class GeneticAlgorithmTest
  {
    private readonly ITestOutputHelper testOutputHelper;

    public GeneticAlgorithmTest(ITestOutputHelper testOutputHelper)
    {
      this.testOutputHelper = testOutputHelper;
    }

    [Fact(Skip = "manual test only")]
    public void ShouldTestGeneticAlgorithm()
    {
      var population = new Population(1000, 10000, new FighterChromosome());
      var fitness = new FighterFitness();

      var ga = RunGeneticAlgorithm(population, fitness, 150);
      var fighter = new AdvancedFighter();
      (ga.BestChromosome as FighterChromosome).ApplyTo(fighter);
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

    private GeneticAlgorithm RunGeneticAlgorithm(Population population, IFitness fitness, int generations)
    {
      var selection = new EliteSelection();
      var crossover = new TwoPointCrossover();
      var mutation = new ReverseSequenceMutation();

      var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
      {
        Termination = new GenerationNumberTermination(generations),
      };

      var latestFitness = 0.0;

      ga.GenerationRan += (sender, e) =>
      {
        var bestfitness = ga.BestChromosome.Fitness ?? 0;
        if (bestfitness != latestFitness)
        {
          latestFitness = bestfitness;

          testOutputHelper.WriteLine($"Current best fitness: {bestfitness}");
        }
      };

      ga.Start();
      return ga;
    }
  }
}
