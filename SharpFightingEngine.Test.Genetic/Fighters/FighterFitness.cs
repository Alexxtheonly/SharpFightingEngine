using System;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using SharpFightingEngine.Battlefields.Bounds;
using SharpFightingEngine.Battlefields.Plain;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.FighterPositionGenerators;
using SharpFightingEngine.Engines.MoveOrders;
using SharpFightingEngine.Features;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.WinConditions;

namespace SharpFightingEngine.Test.Genetic
{
  public class FighterFitness : IFitness
  {
    private const int Min = 1;
    private const int Max = 25;

    private readonly Random random = new Random();

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

      var fighters = new List<IFighterStats>(GetFighters(9))
      {
        geneticFighter
      };

      var powerlevels = fighters
        .Select(o => new { Fighter = o.Id, PowerLevel = o.PowerLevel() })
        .OrderByDescending(o => o.PowerLevel);

      var battlefield = new PlainBattlefield(new Small());

      var features = new List<IEngineFeature>()
      {
        new FeatureRegenerateEnergy(),
        new FeatureRegenerateHealth(),
      };

      Engine engine = new Engine(
        cfg =>
      {
        cfg.ActionsPerRound = 1;
        cfg.Battlefield = battlefield;
        cfg.Features = features;
        cfg.MoveOrder = new AllRandomMoveOrder();
        cfg.PositionGenerator = new AllRandomPositionGenerator();
        cfg.WinCondition = new LastManStandingWinCondition();

        return cfg;
      }, fighters);

      var result = engine.StartMatch();
      var score = result.Scores.First(o => o.Id == geneticFighter.Id);

      var fitness = score.TotalDeaths * -1000;
      fitness += score.TotalKills * 100;
      fitness += score.TotalDamageDone * 10;
      fitness += score.TotalDamageTaken * -1;

      var powerlevel = geneticFighter.PowerLevel();

      return fitness;
    }

    private IEnumerable<IFighterStats> GetFighters(int count)
    {
      for (int i = 0; i < count; i++)
      {
        yield return new GenericFighter()
        {
          Id = Guid.NewGuid(),
          Accuracy = GetRandomValue(),
          Agility = GetRandomValue(),
          Toughness = GetRandomValue(),
          Power = GetRandomValue(),
          Regeneration = GetRandomValue(),
          Speed = GetRandomValue(),
          Stamina = GetRandomValue(),
          Vision = GetRandomValue(),
          Vitality = GetRandomValue(),
          Expertise = GetRandomValue(),
        };
      }
    }

    private int GetRandomValue()
    {
      return random.Next(Min, Max);
    }
  }
}
