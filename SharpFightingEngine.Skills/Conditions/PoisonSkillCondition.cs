using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Skills.Conditions
{
  public class PoisonSkillCondition : ISkillCondition
  {
    private const int Duration = 3;

    public Guid Id => new Guid("2E2767F9-A7D9-4D67-B0FC-D6A1B6F258C8");

    public string Name => "Poison";

    public bool PreventsPerformingActions => false;

    public float? HealingReduced => 0.66F;

    public int Damage => 5;

    public int Remaining { get; set; } = Duration;

    public int Initial => Duration;

    public void Apply(IStats stats)
    {
    }

    public IEnumerable<EngineTick> Apply(IFighterStats fighter)
    {
      fighter.DamageTaken += Damage;

      return new FighterConditionDamageTick()
      {
        Condition = this.AsStruct(),
        Damage = Damage,
        Fighter = fighter,
      }.Yield();
    }
  }
}
