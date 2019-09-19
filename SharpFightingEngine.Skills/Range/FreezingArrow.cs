using System;
using SharpFightingEngine.Skills.Melee;

namespace SharpFightingEngine.Skills.Range
{
  public class FreezingArrow : FreezingSmash
  {
    public override Guid Id => new Guid("185334D3-F150-493F-A69B-419417A41F2A");

    public override string Name => "Freezing Arrow";

    public override float Range => 15;
  }
}
