using System.Numerics;

namespace SharpFightingEngine.Test.Data.Combat
{
  public class AttackInRangeTheoryData : TheoryData
  {
    public AttackInRangeTheoryData()
    {
      AddRow(new Vector3(0, 0, 0), new Vector3(0, 5, 0), 5, true);
      AddRow(new Vector3(0, 0, 0), new Vector3(5, 5, 0), 5, false);
    }
  }
}
