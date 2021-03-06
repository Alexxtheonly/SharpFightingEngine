﻿using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills;

namespace SharpFightingEngine.Combat
{
  public class Attack : IAttack
  {
    public IDamageSkill Skill { get; set; }

    public IFighterStats Target { get; set; }

    public IFighterStats Actor { get; set; }
  }
}
