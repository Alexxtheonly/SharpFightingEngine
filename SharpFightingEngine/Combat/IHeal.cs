using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Combat
{
  public interface IHeal : IFighterAction
  {
    IFighterStats Target { get; }

    IHealSkill Skill { get; }
  }
}
