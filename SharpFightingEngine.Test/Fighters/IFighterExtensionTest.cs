using System.Collections.Generic;
using Moq;
using SharpFightingEngine.Combat;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills;
using SharpFightingEngine.Skills.Conditions;
using Xunit;

namespace SharpFightingEngine.Test.Fighters
{
  public class IFighterExtensionTest
  {
    private static readonly EngineCalculationValues Values = new EngineCalculationValues();

    [Fact]
    public void ConditionShouldReduceHeal()
    {
      var fighter = new Mock<IFighterStats>();

      var condition = new Mock<ISkillCondition>();
      condition
        .Setup(o => o.HealingReduced)
        .Returns(0.5F);

      fighter
        .Setup(o => o.DamageTaken)
        .Returns(200);

      fighter
        .Setup(o => o.States)
        .Returns(new List<IExpiringState>()
        {
          condition.Object,
        });

      fighter
        .Setup(o => o.GetAdjustedStats().HealingPower)
        .Returns(0);

      var heal = new Mock<IHeal>();
      heal
        .Setup(o => o.Actor)
        .Returns(fighter.Object);

      heal
        .Setup(o => o.Skill.Heal)
        .Returns(30);

      var expected = (int)(30 * Values.HealingPowerFactor * 0.5F);
      var actual = fighter.Object.Heal(heal.Object.GetHealValue(Values));

      Assert.Equal(expected, actual);
    }
  }
}
