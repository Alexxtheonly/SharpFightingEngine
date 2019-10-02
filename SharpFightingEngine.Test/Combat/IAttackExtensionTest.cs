using System.Numerics;
using AutoFixture.Xunit2;
using Moq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Combat;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Test.Data.Combat;
using Xunit;

namespace SharpFightingEngine.Test.Combat
{
  public class IAttackExtensionTest
  {
    private readonly Mock<IAttack> attack = new Mock<IAttack>();
    private readonly EngineCalculationValues calculationValues = new EngineCalculationValues();

    [Theory]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    public void HitChanceShouldBeCorrect(int accuracy)
    {
      attack
        .Setup(o => o.Actor.GetAdjustedStats().Accuracy)
        .Returns(accuracy);

      Assert.Equal(100 + (attack.Object.Actor.GetAdjustedStats().Accuracy * calculationValues.AccuracyFactor), attack.Object.HitChance(calculationValues));
    }

    [Theory]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    public void DodgechanceShouldBeCorrect(int agility)
    {
      attack
        .Setup(o => o.Target.GetAdjustedStats().Agility)
        .Returns(agility);

      Assert.Equal(attack.Object.Target.GetAdjustedStats().Agility * calculationValues.AgilityFactor, attack.Object.DodgeChance(calculationValues));
    }

    [Theory]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    public void DamageShouldBeCorrect(int skillDamage, int power, int toughness)
    {
      attack
        .Setup(o => o.Skill.Damage)
        .Returns(skillDamage);

      attack
        .Setup(o => o.Actor.GetAdjustedStats().Power)
        .Returns(power);

      attack
        .Setup(o => o.Target.GetAdjustedStats().Armor)
        .Returns(toughness);

      var expected = (int)(attack.Object.Skill.Damage * attack.Object.Actor.GetAdjustedStats().Level * ((attack.Object.Actor.GetAdjustedStats().Power * calculationValues.AttackPowerFactor) / (attack.Object.Target.GetAdjustedStats().Armor * calculationValues.ArmorFactor)));

      Assert.Equal(expected, attack.Object.GetDamage(calculationValues));
    }

    [Theory]
    [ClassData(typeof(AttackInRangeTheoryData))]
    public void ShouldGetInRange(Vector3 position, Vector3 targetPosition, float range, bool expected)
    {
      var actor = Mock.Of<IFighterStats>();
      actor.Set(position);

      var target = Mock.Of<IFighterStats>();
      target.Set(targetPosition);

      var attack = new Mock<IAttack>();
      attack
        .Setup(o => o.Actor)
        .Returns(actor);

      attack
        .Setup(o => o.Target)
        .Returns(target);

      attack
        .Setup(o => o.Skill.Range)
        .Returns(range);

      var actual = attack.Object.IsInRange();
      Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(AttackGetDistanceTheoryData))]
    public void ShouldGetDistance(IAttack attack)
    {
      var expected = attack.Actor.GetDistance(attack.Target);
      var actual = attack.GetDistance();

      Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, 1, 100)]
    [InlineData(0, 100, 0)]
    public void HitValueShouldBeCorrect(int accuracy, int agility, float expected)
    {
      attack
        .Setup(o => o.Actor.GetAdjustedStats().Accuracy)
        .Returns(accuracy);

      attack
        .Setup(o => o.Target.GetAdjustedStats().Agility)
        .Returns(agility);

      var hitvalue = attack.Object.HitValue(calculationValues);
      Assert.Equal(expected, hitvalue);
    }
  }
}
