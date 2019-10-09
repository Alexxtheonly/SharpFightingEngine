namespace SharpFightingEngine.Skills.Buffs
{
  public static class ISkillBuffExtension
  {
    public static SkillBuff AsStruct(this ISkillBuff buff)
    {
      return new SkillBuff()
      {
        Id = buff.Id,
        Name = buff.Name,
        ReflectChance = buff.ReflectChance,
        Remaining = buff.Remaining,
      };
    }
  }
}
