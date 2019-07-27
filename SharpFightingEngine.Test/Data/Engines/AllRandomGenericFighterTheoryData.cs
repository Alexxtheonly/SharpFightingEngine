using SharpFightingEngine.Test.Utilities;

namespace SharpFightingEngine.Test.Data.Engines
{
  public class AllRandomGenericFighterTheoryData : TheoryData
  {
    public AllRandomGenericFighterTheoryData()
    {
      AddRow(Utility.GetDefaultEngine(20, 300));
    }
  }
}
