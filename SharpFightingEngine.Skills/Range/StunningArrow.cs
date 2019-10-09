using System;
using SharpFightingEngine.Skills.Melee;

namespace SharpFightingEngine.Skills.Range
{
  public class StunningArrow : StunningSmash
  {
    public override Guid Id => new Guid("978DDAA0-6B74-4672-8E27-8704FE3AEC1A");

    public override string Name => "Stunning Arrow";

    public override float Range => 15;

    public override bool CanBeReflected => true;
  }
}
