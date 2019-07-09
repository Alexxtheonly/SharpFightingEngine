using AutoFixture;
using AutoFixture.AutoMoq;
using SharpFightingEngine.Combat;

namespace SharpFightingEngine.Test.Data.Combat
{
  public class AttackGetDistanceTheoryData : TheoryData
  {
    public AttackGetDistanceTheoryData()
    {
      var fixture = new Fixture();
      fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true });

      for (int i = 0; i < 50; i++)
      {
        AddRow(fixture.Create<IAttack>());
      }
    }
  }
}
