using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.Conditions
{
  public class CrippleSkillCondition : ISkillCondition
  {
    private const int Duration = 2;

    public Guid Id => new Guid("7233ECE4-999D-45E5-8FF8-7D9AB4587DC9");

    public string Name => "Cripple";

    public bool PreventsPerformingActions => false;

    public float? HealingReduced => null;

    public int Damage => 0;

    public int Remaining { get; set; } = Duration;

    public int Initial => Duration;

    public void Apply(IStats stats)
    {
      stats.Speed *= 0.5F;
    }

    public IEnumerable<EngineTick> Apply(IFighterStats fighter)
    {
      return Enumerable.Empty<EngineTick>();
    }
  }
}
