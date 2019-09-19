using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Skills.Conditions
{
  public class StunSkillCondition : ISkillCondition
  {
    private const int Duration = 2;

    public Guid Id => new Guid("09FBD6B5-3F0D-4DE5-B40B-D7D9C7AA7A92");

    public string Name => "Stun";

    public bool PreventsPerformingActions => true;

    public float? HealingReduced => null;

    public int Damage => 0;

    public int Remaining { get; set; } = Duration;

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
