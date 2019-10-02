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
  public class CripplingSmash : DamageSkillBase
  {
    public override Guid Id => new Guid("41E9B654-BCB2-454C-A8FA-E68C7B21C46A");

    public override string Name => "Crippling Smash";

    public override int DamageLow => 5;

    public override int DamageHigh => 8;

    public override float Range => 1;

    public override int Cooldown => 1;

    public override IEnumerable<EngineTick> Perform(IFighterStats actor, IFighterStats target, EngineCalculationValues calculationValues)
    {
      if (25F.Chance())
      {
        return Enumerable.Empty<EngineTick>();
      }

      var condition = new CrippleSkillCondition(actor);

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
