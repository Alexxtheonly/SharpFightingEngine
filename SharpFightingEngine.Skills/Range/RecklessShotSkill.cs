using System;

namespace SharpFightingEngine.Skills.Range
{
  public class RecklessShotSkill : ISkill
  {
    public Guid Id => new Guid("69D6E8C5-3FAE-4602-A66C-236A5C4271C7");

    public string Name => "Reckless Shot";

    public int Damage => Energy;

    public float Range => 3.5F;

    public int Energy => 9;
  }
}
