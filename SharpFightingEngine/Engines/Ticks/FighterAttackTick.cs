using SharpFightingEngine.Combat;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterAttackTick : FighterTick
  {
    public IFighter Target { get; set; }

    public IAttack Attack { get; set; }

    public ISkill Skill => Attack.Skill;

    public int Damage { get; set; }

    public int Energy => Skill.Energy;

    public float Distance => Attack.GetDistance();

    public bool Critical { get; set; }

    public bool Dodged { get; set; }

    public bool OutOfRange { get; set; }

    public bool InsufficientEnergy { get; set; }

    public bool Hit => !(Dodged || OutOfRange || InsufficientEnergy);

    public override string ToString()
    {
      return $"{base.ToString()} attacking {Target.Id} with {Attack.Skill.Name} damage {Damage} " +
        $"{(Critical ? "critical" : string.Empty)}" +
        $"{(Dodged ? "dodged" : string.Empty)} " +
        $"{(OutOfRange ? "out of range" : string.Empty)} " +
        $"{(InsufficientEnergy ? "insufficient energy" : string.Empty)}";
    }
  }
}
