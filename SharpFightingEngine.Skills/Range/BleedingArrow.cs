using System;
using SharpFightingEngine.Skills.Melee;

namespace SharpFightingEngine.Skills.Range
{
  public class BleedingArrow : BleedingSmash
  {
    public override Guid Id => new Guid("9405E881-815B-459F-849E-ECB646BA0153");

    public override string Name => "Bleeding Arrow";

    public override float Range => 15;

    public override bool CanBeReflected => true;
  }
}
