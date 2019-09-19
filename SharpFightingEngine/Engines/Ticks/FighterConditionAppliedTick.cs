using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills.Conditions;

namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterConditionAppliedTick : FighterTick
  {
    public IFighter Target { get; set; }

    public ISkillCondition Condition { get; set; }

    public override string ToString()
    {
      return $"{base.ToString()} applied {Condition.Name} on {Target.Id}";
    }
  }
}
