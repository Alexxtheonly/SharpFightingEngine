using System.Linq;
using Moq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Battlefields.Bounds;
using SharpFightingEngine.Fighters.Algorithms.PathFinders;
using Xunit;

namespace SharpFightingEngine.Fighters.Test.Algorithms.Pathfinders
{
  public class DefaultPathFinderTest
  {
    private readonly DefaultPathFinder defaultPathFinder = new DefaultPathFinder();
    private readonly Mock<IBattlefield> battlefield = new Mock<IBattlefield>();

    public DefaultPathFinderTest()
    {
      battlefield
        .Setup(o => o.CurrentBounds)
        .Returns(new Small());
    }

    [Fact]
    public void ShouldReturnValidEscapePathEasy()
    {
      var enemies = new Position[]
      {
        new Position()
        {
          X = 11,
          Y = 11,
        },
        new Position()
        {
          X = 19,
          Y = 18,
        },
      }.Cast<IPosition>();

      var own = new Position()
      {
        X = 15,
        Y = 16,
      };

      var escapePosition = defaultPathFinder.GetEscapePath(own, enemies, battlefield.Object);

      foreach (var enemy in enemies)
      {
        var distance = escapePosition.GetDistanceAbs(enemy);
        Assert.InRange(distance, 18, 22);
      }
    }

    [Fact]
    public void ShouldReturnValidEscapePath()
    {
      var enemies = new Position[]
      {
        new Position()
        {
          X = 57.9350471F,
          Y = 56.9235649F,
        },
        new Position()
        {
          X = 50.3793526F,
          Y = 55.40332F,
        },
      }.Cast<IPosition>();

      var own = new Position()
      {
        X = 57.3367462F,
        Y = 58.3573265F,
      };

      var escapePosition = defaultPathFinder.GetEscapePath(own, enemies, battlefield.Object);

      foreach (var enemy in enemies)
      {
        var distance = escapePosition.GetDistanceAbs(enemy);
        Assert.InRange(distance, 18, 22);
      }
    }
  }
}
