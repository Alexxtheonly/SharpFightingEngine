namespace SharpFightingEngine.Skills
{
  public interface IDamageSkill : ISkill
  {
    int Damage { get; }

    bool CanBeReflected { get; }
  }
}
