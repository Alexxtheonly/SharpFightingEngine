using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterHealTick : FighterSkillTick
  {
    public IFighter Target { get; set; }

    public override ISkill Skill => HealSkill;

    public IHealSkill HealSkill { get; set; }

    public int PotentialHealing { get; set; }

    public int AppliedHealing { get; set; }

    public bool OutOfRange { get; set; }

    public bool OnCooldown { get; set; }

    public override string ToString()
    {
      return $"{base.ToString()} heal {Target.Id} for {AppliedHealing} health" +
        $"{(OutOfRange ? "out of range" : string.Empty)} " +
        $"{(OnCooldown ? "on cooldown" : string.Empty)}";
    }
  }
}
