using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Skills.Conditions
{
  public class BleedSkillCondition : ISkillCondition
  {
    private const int Duration = 3;

    public Guid Id => new Guid("F84C756E-CE75-4F65-9724-2F289231EC1B");

    public string Name => "Bleed";

    public bool PreventsPerformingActions => false;

    public float? HealingReduced => null;

    public int Remaining { get; set; } = Duration;

    public int Initial => Duration;

    public int Damage => 8;

    public void Apply(IStats stats)
    {
    }

    public IEnumerable<EngineTick> Apply(IFighterStats fighter)
    {
      fighter.DamageTaken += Damage;

      return new FighterConditionDamageTick()
      {
        Condition = this.AsStruct(),
        Fighter = fighter,
        Damage = Damage,
      }.Yield();
    }
  }
}
