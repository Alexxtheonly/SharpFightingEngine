namespace SharpFightingEngine.Skills.Conditions
{
  public interface ISkillCondition : IExpiringState
  {
    bool PreventsPerformingActions { get; }

    float? HealingReduced { get; }

    int Damage { get; }
  }
}
