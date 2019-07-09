using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using SharpFightingEngine.Engines;

namespace SharpFightingEngine.Test.Genetic.EngineCalculation
{
  public class EngineCalculationChromosome : ChromosomeBase
  {
    private static readonly IEnumerable<PropertyInfo> Properties = typeof(EngineCalculationValues).GetProperties();

    public EngineCalculationChromosome()
      : base(Properties.Count())
    {
      CreateGenes();
    }

    public override IChromosome CreateNew()
    {
      return new EngineCalculationChromosome();
    }

    public override Gene GenerateGene(int geneIndex)
    {
      return new Gene(RandomizationProvider.Current.GetFloat(0, 5));
    }

    public override IChromosome Clone()
    {
      return base.Clone() as EngineCalculationChromosome;
    }

    public void ApplyTo(EngineCalculationValues calculationValues)
    {
      for (int i = 0; i < Properties.Count(); i++)
      {
        var value = GetGene(i).Value;
        var property = Properties.ElementAt(i);
        property.SetValue(calculationValues, value);
      }
    }
  }
}
