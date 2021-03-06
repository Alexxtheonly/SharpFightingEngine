﻿using System;
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
  public class StunningSmash : DamageSkillBase
  {
    public override Guid Id => new Guid("06917420-8690-4485-84CC-6FD01E70FD48");

    public override string Name => "Stunning Smash";

    public override int DamageLow => 5;

    public override int DamageHigh => 8;

    public override float Range => 1;

    public override int Cooldown => 2;

    public override bool CanBeReflected => false;

    public override IEnumerable<EngineTick> Perform(IFighterStats actor, IFighterStats target, EngineCalculationValues calculationValues)
    {
      if (80F.Chance())
      {
        return Enumerable.Empty<EngineTick>();
      }

      var condition = new StunSkillCondition(actor);

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
