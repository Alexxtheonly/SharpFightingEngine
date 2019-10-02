using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Combat
{
  public class Heal : IHeal
  {
    public IFighterStats Target { get; set; }

    public IHealSkill Skill { get; set; }

    public IFighterStats Actor { get; set; }
  }
}
