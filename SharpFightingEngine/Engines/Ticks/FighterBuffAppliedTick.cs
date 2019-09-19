using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills.Buffs;

namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterBuffAppliedTick : FighterTick
  {
    public IFighterStats Target { get; set; }

    public ISkillBuff Buff { get; set; }
  }
}
