using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Test.Genetic
{
  public class FighterChromosome : ChromosomeBase
  {
    private static readonly IEnumerable<PropertyInfo> FighterProperties =
      typeof(IOffensiveStats).GetProperties()
      .Union(typeof(IDefensiveStats).GetProperties())
      .Union(typeof(IUtilityStats).GetProperties());

    public FighterChromosome()
      : base(FighterProperties.Count())
    {
      CreateGenes();
    }

    public override IChromosome CreateNew()
    {
      return new FighterChromosome();
    }

    public override Gene GenerateGene(int geneIndex)
    {
      return new Gene(RandomizationProvider.Current.GetInt(1, 300));
    }

    public override IChromosome Clone()
    {
      return base.Clone() as FighterChromosome;
    }

    public void ApplyTo(FighterBase fighter)
    {
      for (int i = 0; i < FighterProperties.Count(); i++)
      {
        var value = GetGene(i).Value;
        var property = typeof(FighterBase).GetProperty(FighterProperties.ElementAt(i).Name);
        property.SetValue(fighter, value);
      }
    }
  }
}
