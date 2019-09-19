using System.Collections.Generic;
using Moq;
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
    public void ConditionShouldReduceRegeneration()
    {
      var fighter = new Mock<IFighterStats>();

      var condition = new Mock<ISkillCondition>();
      condition
        .Setup(o => o.HealingReduced)
        .Returns(0.5F);

      fighter
        .Setup(o => o.States)
        .Returns(new List<IExpiringState>()
        {
          condition.Object,
        });

      fighter
        .Setup(o => o.GetAdjustedStats().Regeneration)
        .Returns(30);

      var expected = (int)((30 * Values.HealthRegenerationFactor) * 0.5F);
      var actual = fighter.Object.HealthRegeneration(Values);

      Assert.Equal(expected, actual);
    }
  }
}
