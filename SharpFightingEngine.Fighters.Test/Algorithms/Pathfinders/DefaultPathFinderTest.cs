using Moq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Battlefields.Bounds;
using SharpFightingEngine.Fighters.Algorithms.PathFinders;
using SharpFightingEngine.Fighters.Test.Data.Algorithms.Pathfinders;
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

    [Theory]
    [ClassData(typeof(PathFinderTheoryData))]
    public void ShouldReturnValidEscapePath(IPosition nearestEnemy, IPosition furthestEnemy, IPosition position)
    {
      var enemies = new IPosition[] { nearestEnemy, furthestEnemy };
      var escapePosition = defaultPathFinder.GetEscapePath(position, enemies, battlefield.Object);

      foreach (var enemy in enemies)
      {
        var distance = escapePosition.GetDistanceAbs(enemy);
        Assert.InRange(distance, 18, 22);
      }
    }
  }
}
