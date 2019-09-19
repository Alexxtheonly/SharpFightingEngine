using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.Conditions
{
  public class FreezeSkillCondition : ISkillCondition
  {
    private const int Duration = 1;

    public Guid Id => new Guid("8DDD3B8E-25A4-46C2-BCCE-031C21CB396E");

    public string Name => "Freeze";

    public bool PreventsPerformingActions => true;

    public float? HealingReduced => null;

    public int Damage => 0;

    public int Remaining { get; set; }

    public int Initial => Duration;

    public void Apply(IStats stats)
    {
    }

    public IEnumerable<EngineTick> Apply(IFighterStats fighter)
    {
      return Enumerable.Empty<EngineTick>();
    }
  }
}
