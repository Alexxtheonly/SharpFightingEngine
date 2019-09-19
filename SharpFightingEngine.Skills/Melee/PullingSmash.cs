using System;
using System.Collections.Generic;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills.General;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Skills.Melee
{
  public class PullingSmash : SkillBase
  {
    public override Guid Id => new Guid("53AF2847-F9FC-4C1A-9E76-04106E07674D");

    public override string Name => "Pulling Smash";

    public override int DamageLow => 12;

    public override int DamageHigh => 18;

    public override float Range => 15;

    public override int Energy => 20;

    public override int Cooldown => 2;

    public override IEnumerable<EngineTick> Perform(IFighterStats actor, IFighterStats target, EngineCalculationValues calculationValues)
    {
      var movedBySkill = new FighterMovedByAttackTick()
      {
        Current = target.AsStruct(),
        Fighter = actor.AsStruct(),
        Target = target.AsStruct(),
      };

      target.Set(actor.CalculatePullPosition(target, 14));

      movedBySkill.Next = target.AsStruct();

      return movedBySkill.Yield();
    }
  }
}
