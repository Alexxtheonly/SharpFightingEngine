using System.Numerics;
using AutoFixture.Xunit2;
using Moq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Combat;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Fighters;
using SharpFightingEngine.Skills;
using SharpFightingEngine.Test.Data.Combat;
using Xunit;

namespace SharpFightingEngine.Test.Combat
{
  public class IAttackExtensionTest
  {
    private readonly Mock<IAttack> attack = new Mock<IAttack>();
    private readonly EngineCalculationValues calculationValues = new EngineCalculationValues();

    [Theory]
    [InlineData(5, 20, true)]
    [InlineData(1, 1, true)]
    [InlineData(20, 5, false)]
    [InlineData(0, 0, true)]
    [InlineData(30, 31, true)]
    [InlineData(50, 5, false)]
    public void ShouldValidateEnergy(int energy, int available, bool expected)
    {
      var skill = new Mock<ISkill>();
      skill
        .Setup(o => o.Energy)
        .Returns(energy);

      attack
        .Setup(o => o.Skill)
        .Returns(skill.Object);

      attack
        .Setup(o => o.Actor.Stamina)
        .Returns(available / calculationValues.StaminaFactor);

      Assert.Equal(expected, attack.Object.HasEnoughEnergy(calculationValues));
    }

    [Theory]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    public void HitChanceShouldBeCorrect(float accuracy)
    {
      attack
        .Setup(o => o.Actor.Accuracy)
        .Returns(accuracy);

      Assert.Equal(100 + (attack.Object.Actor.Accuracy * calculationValues.AccuracyFactor), attack.Object.HitChance(calculationValues));
    }

    [Theory]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    public void DodgechanceShouldBeCorrect(float agility)
    {
      attack
        .Setup(o => o.Target.Agility)
        .Returns(agility);

      Assert.Equal(attack.Object.Target.Agility * calculationValues.AgilityFactor, attack.Object.DodgeChance(calculationValues));
    }

    [Theory]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    public void DamageShouldBeCorrect(int skillDamage, float power, float toughness)
    {
      attack
        .Setup(o => o.Skill.Damage)
        .Returns(skillDamage);

      attack
        .Setup(o => o.Actor.Power)
        .Returns(power);

      attack
        .Setup(o => o.Target.Toughness)
        .Returns(toughness);

      var expected = (int)(attack.Object.Skill.Damage * ((attack.Object.Actor.Power * calculationValues.AttackPowerFactor) / (attack.Object.Target.Toughness * calculationValues.ArmorDefenseFactor)));

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
    [InlineData(1, 1, 100.7)]
    [InlineData(1, 200, 1.199997)]
    public void HitValueShouldBeCorrect(int accuracy, int agility, float expected)
    {
      attack
        .Setup(o => o.Actor.Accuracy)
        .Returns(accuracy);

      attack
        .Setup(o => o.Target.Agility)
        .Returns(agility);

      var hitvalue = attack.Object.HitValue(calculationValues);
      Assert.Equal(expected, hitvalue);
    }
  }
}
