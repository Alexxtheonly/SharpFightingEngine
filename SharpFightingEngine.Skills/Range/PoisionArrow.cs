using System;
using SharpFightingEngine.Skills.Melee;

namespace SharpFightingEngine.Skills.Range
{
  public class PoisionArrow : PoisonSmash
  {
    public override Guid Id => new Guid("A9954807-D485-4E7A-9830-7C1356922F89");

    public override string Name => "Poison Arrow";

    public override float Range => 15;
  }
}
