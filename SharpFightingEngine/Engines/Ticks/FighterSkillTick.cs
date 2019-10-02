using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Engines.Ticks
{
  public abstract class FighterSkillTick : FighterTick
  {
    public abstract ISkill Skill { get; }
  }
}
