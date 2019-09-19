using SharpFightingEngine.Fighters.Factories;
using Xunit;

namespace SharpFightingEngine.Fighters.Test.Factories
{
  public class FighterFactoryTest
  {
    [Theory]
    [InlineData(100)]
    [InlineData(200)]
    [InlineData(500)]
    [InlineData(1000)]
    public void ShouldReturnFighterWithMatchingPowerlevel(int powerlevel)
    {
      var fighter = FighterFactory.GetFighter(powerlevel);

      Assert.Equal(powerlevel, fighter.Stats.PowerLevel());
    }
  }
}
