using System;
using SharpFightingEngine.Skills.Melee;

namespace SharpFightingEngine.Skills.Range
{
  public class BurningArrow : BurningSmash
  {
    public override Guid Id => new Guid("1E4D0975-76DC-4020-98B9-76B9134E0DD6");

    public override string Name => "Burning Arrow";

    public override float Range => 15;

    public override bool CanBeReflected => true;
  }
}
