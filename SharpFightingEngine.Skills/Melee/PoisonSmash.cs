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
  public class PoisonSmash : DamageSkillBase
  {
    public override Guid Id => new Guid("36904734-786D-48AD-A9C7-63EE38810F89");

    public override string Name => "Poison Smash";

    public override int DamageLow => 5;

    public override int DamageHigh => 8;

    public override float Range => 1;

    public override int Cooldown => 1;

    public override bool CanBeReflected => false;

    public override IEnumerable<EngineTick> Perform(IFighterStats actor, IFighterStats target, EngineCalculationValues calculationValues)
    {
      if (70F.Chance())
      {
        return Enumerable.Empty<EngineTick>();
      }

      var condition = new PoisonSkillCondition(actor);
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
