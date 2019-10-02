using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills.Conditions;
using SharpFightingEngine.Skills.General;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Skills.Melee
{
  public class BurningSmash : DamageSkillBase
  {
    public override Guid Id => new Guid("483007BD-DC6D-400C-A570-01AC8A7F91D8");

    public override string Name => "Burning Smash";

    public override int DamageLow => 5;

    public override int DamageHigh => 8;

    public override float Range => 1;

    public override int Cooldown => 2;

    public override IEnumerable<EngineTick> Perform(IFighterStats actor, IFighterStats target, EngineCalculationValues calculationValues)
    {
      if (80F.Chance())
      {
        return Enumerable.Empty<EngineTick>();
      }

      var condition = new BurnSkillCondition(actor);

      target.States.Add(condition);

      return new FighterConditionAppliedTick()
      {
        Condition = condition,
        Fighter = actor.AsStruct(),
        Target = target.AsStruct(),
      }.Yield();
    }
  }
}
