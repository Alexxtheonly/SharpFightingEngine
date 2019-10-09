namespace SharpFightingEngine.Skills.Conditions
{
  public static class ISkillConditionExtension
  {
    public static SkillCondition AsStruct(this ISkillCondition condition)
    {
      return new SkillCondition()
      {
        Damage = condition.Damage,
        HealingReduced = condition.HealingReduced,
        Id = condition.Id,
        Name = condition.Name,
        PreventsPerformingActions = condition.PreventsPerformingActions,
        Remaining = condition.Remaining,
      };
    }
  }
}
