using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills.Conditions;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Skills.Extensions
{
  public static class IFighterStatsExtension
  {
    public static FighterMovedByAttackTick ApplyKnockback(this IFighterStats target, IFighterStats actor, float distance)
    {
      return target.ApplyMove(actor, actor.CalculateKnockBackPosition(target, distance));
    }

    public static FighterMovedByAttackTick ApplyPull(this IFighterStats target, IFighterStats actor, float distance)
    {
      return target.ApplyMove(actor, actor.CalculatePullPosition(target, distance));
    }

    public static FighterMovedByAttackTick ApplyCharge(this IFighterStats target, IFighterStats actor, float distance)
    {
      return target.ApplyMove(actor, actor.CalculateChargePosition(target, distance));
    }

    public static FighterMovedByAttackTick ApplyMove(this IFighterStats target, IFighterStats actor, IPosition newPosition)
    {
      var movedBySkill = new FighterMovedByAttackTick()
      {
        Current = target.AsStruct(),
        Fighter = actor.AsStruct(),
        Target = target.AsStruct(),
      };

      target.Set(newPosition);

      movedBySkill.Next = target.AsStruct();

      return movedBySkill;
    }

    public static FighterConditionAppliedTick ApplyBleeding(this IFighterStats target, IFighterStats actor, float chance)
    {
      return target.ApplyCondition(actor, chance, new BleedSkillCondition(actor));
    }

    public static FighterConditionAppliedTick ApplyBurning(this IFighterStats target, IFighterStats actor, float chance)
    {
      return target.ApplyCondition(actor, chance, new BurnSkillCondition(actor));
    }

    public static FighterConditionAppliedTick ApplyCripple(this IFighterStats target, IFighterStats actor, float chance)
    {
      return target.ApplyCondition(actor, chance, new CrippleSkillCondition(actor));
    }

    public static FighterConditionAppliedTick ApplyFreeze(this IFighterStats target, IFighterStats actor, float chance)
    {
      return target.ApplyCondition(actor, chance, new FreezeSkillCondition(actor));
    }

    public static FighterConditionAppliedTick ApplyPoison(this IFighterStats target, IFighterStats actor, float chance)
    {
      return target.ApplyCondition(actor, chance, new PoisonSkillCondition(actor));
    }

    public static FighterConditionAppliedTick ApplyStun(this IFighterStats target, IFighterStats actor, float chance)
    {
      return target.ApplyCondition(actor, chance, new StunSkillCondition(actor));
    }

    public static FighterConditionAppliedTick ApplyCondition(this IFighterStats target, IFighterStats actor, float chance, SkillConditionBase condition)
    {
      if (!chance.Chance())
      {
        return null;
      }

      target.States.Add(condition);

      return new FighterConditionAppliedTick()
      {
        Condition = condition.AsStruct(),
        Fighter = actor.AsStruct(),
        Target = target.AsStruct(),
      };
    }
  }
}
