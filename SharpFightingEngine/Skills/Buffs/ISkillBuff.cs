﻿namespace SharpFightingEngine.Skills.Buffs
{
  public interface ISkillBuff : IExpiringState
  {
    float? ReflectChance { get; }
  }
}
