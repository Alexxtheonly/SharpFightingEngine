using SharpFightingEngine.Combat;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterAttackTick : FighterSkillTick
  {
    public IFighter Target { get; set; }

    public IFighter OriginalTarget { get; set; }

    public IAttack Attack { get; set; }

    public override ISkill Skill => Attack.Skill;

    public IDamageSkill AttackSkill => Attack.Skill;

    public int Damage { get; set; }

    public float Distance => Attack.GetDistance();

    public bool Critical { get; set; }

    public bool Dodged { get; set; }

    public bool OutOfRange { get; set; }

    public bool OnCooldown { get; set; }

    public bool Reflected { get; set; }

    public bool Hit => !(Dodged || OutOfRange || OnCooldown || Reflected);

    public override string ToString()
    {
      return $"{base.ToString()} attacking {Target.Id} with {AttackSkill.Name} damage {Damage} " +
        $"{(Critical ? "critical" : string.Empty)}" +
        $"{(Dodged ? "dodged" : string.Empty)} " +
        $"{(OutOfRange ? "out of range" : string.Empty)} " +
        $"{(OnCooldown ? "on cooldown" : string.Empty)}" +
        $"{(Reflected ? "reflected" : string.Empty)}";
    }
  }
}
