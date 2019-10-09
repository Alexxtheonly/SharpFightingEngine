using System;
using System.Collections.Generic;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills.General;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Skills.Range
{
  public class KnockbackArrow : DamageSkillBase
  {
    public override Guid Id => new Guid("3D928012-67DA-4248-88DE-53316028B803");

    public override string Name => "Knockback Arrow";

    public override int DamageLow => 5;

    public override int DamageHigh => 8;

    public override float Range => 1;

    public override int Cooldown => 2;

    public override bool CanBeReflected => true;

    public override IEnumerable<EngineTick> Perform(IFighterStats actor, IFighterStats target, EngineCalculationValues calculationValues)
    {
      var movedBySkill = new FighterMovedByAttackTick()
      {
        Current = target.AsStruct(),
        Fighter = actor.AsStruct(),
        Target = target.AsStruct(),
      };

      target.Set(actor.CalculateKnockBackPosition(target, 14));

      movedBySkill.Next = target.AsStruct();

      return movedBySkill.Yield();
    }
  }
}
