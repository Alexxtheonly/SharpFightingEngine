using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Skills.Conditions
{
  public class BurnSkillCondition : ISkillCondition
  {
    private const int Duration = 2;

    public Guid Id => new Guid("4650A573-8AE1-4FA6-AE3C-CA4346B7E31C");

    public string Name => "Burn";

    public bool PreventsPerformingActions => false;

    public float? HealingReduced => null;

    public int Damage => 20;

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
