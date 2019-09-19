using System;
using SharpFightingEngine.Skills.Melee;

namespace SharpFightingEngine.Skills.Range
{
  public class CripplingArrow : CripplingSmash
  {
    public override Guid Id => new Guid("DA06EDE7-16EB-46CB-8ECF-7D8AEBAF0543");

    public override string Name => "Crippling Arrow";

    public override float Range => 15;
  }
}
