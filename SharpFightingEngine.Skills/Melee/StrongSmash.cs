using System;
using System.Collections.Generic;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills.Buffs;
using SharpFightingEngine.Skills.Extensions;
using SharpFightingEngine.Skills.General;

namespace SharpFightingEngine.Skills.Melee
{
  public class StrongSmash : DamageSkillBase
  {
    public override Guid Id => new Guid("58A990AC-A1E2-4C50-A958-3D84690E96BB");

    public override string Name => "Strong Smash";

    public override int DamageLow => 8;

    public override int DamageHigh => 10;

    public override float Range => 1;

    public override int Cooldown => 0;

    public override bool CanBeReflected => false;

    public override IEnumerable<EngineTick> Perform(IFighterStats actor, IFighterStats target, EngineCalculationValues calculationValues)
    {
      return new EngineTick[]
      {
        actor.ApplyBuff(actor, 100, new ReflectSkillBuff()),
      };
    }
  }
}
