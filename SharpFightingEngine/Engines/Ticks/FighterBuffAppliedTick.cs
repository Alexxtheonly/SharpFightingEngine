using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills.Buffs;

namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterBuffAppliedTick : FighterTick
  {
    public IFighter Target { get; set; }

    public ISkillBuff Buff { get; set; }

    public override string ToString()
    {
      return $"{base.ToString()} applied {Buff.Name} on {Target.Id}";
    }
  }
}
