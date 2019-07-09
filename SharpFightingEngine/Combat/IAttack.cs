using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Combat
{
  public interface IAttack : IFighterAction
  {
    ISkill Skill { get; }

    IFighterStats Target { get; }
  }
}
