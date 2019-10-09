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
  public class BleedingSmash : DamageSkillBase
  {
    public override Guid Id => new Guid("6470DC6D-12EB-44F2-A69D-1BDE9469DF9A");

    public override string Name => "Bleeding Smash";

    public override int DamageLow => 5;

    public override int DamageHigh => 8;

    public override float Range => 1;

    public override int Cooldown => 1;

    public override bool CanBeReflected => false;

    public override IEnumerable<EngineTick> Perform(IFighterStats actor, IFighterStats target, EngineCalculationValues calculationValues)
    {
      if (40F.Chance())
      {
        return Enumerable.Empty<EngineTick>();
      }

      var condition = new BleedSkillCondition(actor);
      target.States.Add(condition);

      return new FighterConditionAppliedTick()
      {
        Condition = condition.AsStruct(),
        Fighter = actor.AsStruct(),
        Target = target.AsStruct(),
      }.Yield();
    }
  }
}
