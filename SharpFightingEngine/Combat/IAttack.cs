using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Combat
{
  public interface IAttack : IFighterAction
  {
    IDamageSkill Skill { get; }

    IFighterStats Target { get; set; }
  }
}
