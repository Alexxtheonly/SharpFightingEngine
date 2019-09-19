using System.Numerics;
using AutoFixture.Xunit2;
using Moq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Test.Data.Battlefields;
using Xunit;

namespace SharpFightingEngine.Test.Battlefields
{
  public class IPositionExtensionTest
  {
    [Theory]
    [InlineAutoData]
    [InlineAutoData]
    [InlineAutoData]
    public void ShouldGetDistance(Vector3 left, Vector3 right)
    {
      var one = Mock.Of<IPosition>();
      one.Set(left);

      var other = Mock.Of<IPosition>();
      other.Set(right);

      var expected = Vector3.Distance(left, right);
      var actual = one.GetDistance(other);
      Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(InBoundsTheoryData))]
    public void ShouldGetIsInBounds(Vector3 bound1, Vector3 bound2, Vector3 positionVector, bool expected)
    {
      var bounds = new Mock<IBounds>();
      bounds
        .Setup(o => o.Low)
        .Returns(bound1);

      bounds
        .Setup(o => o.High)
        .Returns(bound2);

      var position = Mock.Of<IPosition>();
      position.Set(positionVector);

      var actual = position.IsInsideBounds(bounds.Object);
      Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(DirectionTheoryData))]
    public void ShouldReturnDirection(Vector3 start, Vector3 end, float distance, Vector3 expected)
    {
      var startPos = Mock.Of<IPosition>();
      startPos.Set(start);

      var endPos = Mock.Of<IPosition>();
      endPos.Set(end);

      var actual = startPos.GetDirection(endPos, distance);
      Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(KnockbackTheoryData))]
    public void ShouldReturnKnockbackPosition(IPosition actor, IPosition target, float distance, IPosition expected)
    {
      var actual = actor.CalculateKnockBackPosition(target, distance);
      Assert.Equal(expected.GetVector2(), actual.GetVector2());
    }

    [Theory]
    [ClassData(typeof(PullTheoryData))]
    public void ShouldReturnPullPosition(IPosition actor, IPosition target, float distance, IPosition expected)
    {
      var actual = actor.CalculatePullPosition(target, distance);
      Assert.Equal(expected.GetVector2(), actual.GetVector2());
    }
  }
}
