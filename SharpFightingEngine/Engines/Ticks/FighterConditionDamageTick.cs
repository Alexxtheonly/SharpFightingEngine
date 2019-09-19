using SharpFightingEngine.Skills.Conditions;

namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterConditionDamageTick : FighterTick
  {
    public ISkillCondition Condition { get; set; }

    public int Damage { get; set; }

    public override string ToString()
    {
      return $"{base.ToString()} {Condition.Name} {Condition.Damage} Remaining:{Condition.Remaining}";
    }
  }
}
