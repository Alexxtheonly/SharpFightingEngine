using System;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Randomizations;
using SharpFightingEngine.Battlefields.Bounds;
using SharpFightingEngine.Battlefields.Plain;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.FighterPositionGenerators;
using SharpFightingEngine.Engines.MoveOrders;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Features;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.WinConditions;

namespace SharpFightingEngine.Test.Genetic.EngineCalculation
{
  public class EngineCalculationFitness : IFitness
  {
    public int SinglePowerLevelMin { get; set; }

    public int SinglePowerLevelMax { get; set; }

    public int FighterCount { get; set; } = 10;

    public int MatchCount { get; set; } = 1;

    public double Evaluate(IChromosome chromosome)
    {
      var engineCalculationChromosome = chromosome as EngineCalculationChromosome;
      if (engineCalculationChromosome == null)
      {
        return 0;
      }

      var battlefield = new PlainBattlefield();

      var features = new List<IEngineFeature>()
      {
      };

      double fitness = 0;
      for (int i = 0; i < MatchCount; i++)
      {
        Engine engine = new Engine(
          cfg =>
        {
          cfg.ActionsPerRound = 1;
          cfg.Battlefield = battlefield;
          cfg.Bounds = new Small();
          cfg.Features = features;
          cfg.MoveOrder = new AllRandomMoveOrder();
          cfg.PositionGenerator = new AllRandomPositionGenerator();
          cfg.WinCondition = new LastManStandingWinCondition();
          engineCalculationChromosome.ApplyTo(cfg.CalculationValues);

          return cfg;
        }, GetFighters(FighterCount));

        fitness += GetFitness(engine);
      }

      return fitness / MatchCount;
    }

    private double GetFitness(Engine engine)
    {
      var result = engine.StartMatch();

      var allTicks = result.Ticks.SelectMany(o => o.Ticks);

      var totalDamageTaken = result.Scores.Sum(o => o.TotalDamageTaken);
      var totalMaxHealth = result.Scores.Sum(o => o.MaxHealth);
      var totalDamageDone = result.Scores.Sum(o => o.TotalDamageDone);

      var unsuccessfulAttackCount = allTicks.OfType<FighterAttackTick>().Where(o => !o.Hit).Count();
      var successfulAttackCount = allTicks.OfType<FighterAttackTick>().Where(o => o.Hit).Count();

      // var regenerationPoints = (totalDamageDone - totalRegeneratedHealth) * 2;
      // var energyRegPoints = ((totalEnergyUsed / 2) - totalRegeneratedEnergy) * 2;
      // var damageHealthRelationPoints = (totalDamageDone - totalMaxHealth) * 3;
      var roundPoints = result.Ticks.Count() * 3;
      var attackPoints = (successfulAttackCount - unsuccessfulAttackCount) * 4;
      var fighterDeadPoints = (FighterCount - result.Scores.Count(o => o.TotalDeaths == 0)) * 16;

      var fitness = roundPoints + attackPoints + fighterDeadPoints;

      if (result.Scores.Count(o => o.TotalDeaths == 0) == 1)
      {
        fitness += 3000;
      }

      return fitness;
    }

    private IEnumerable<IFighterStats> GetFighters(int count)
    {
      for (int i = 0; i < count; i++)
      {
        yield return new GenericFighter()
        {
          Id = Guid.NewGuid(),
          Stats = new Stats()
          {
            Accuracy = RandomizationProvider.Current.GetInt(SinglePowerLevelMin, SinglePowerLevelMax),
            Agility = RandomizationProvider.Current.GetInt(SinglePowerLevelMin, SinglePowerLevelMax),
            Armor = RandomizationProvider.Current.GetInt(SinglePowerLevelMin, SinglePowerLevelMax),
            Power = RandomizationProvider.Current.GetInt(SinglePowerLevelMin, SinglePowerLevelMax),
            Speed = RandomizationProvider.Current.GetInt(SinglePowerLevelMin, SinglePowerLevelMax),
            Vision = RandomizationProvider.Current.GetInt(SinglePowerLevelMin, SinglePowerLevelMax),
            Vitality = RandomizationProvider.Current.GetInt(SinglePowerLevelMin, SinglePowerLevelMax),
          },
        };
      }
    }
  }
}
