using System.Numerics;

namespace SharpFightingEngine.Test.Data.Battlefields
{
  public class DirectionTheoryData : TheoryData
  {
    public DirectionTheoryData()
    {
      AddRow(new Vector3(0, 5, 0), new Vector3(10, 5, 0), 5, new Vector3(5, 5, 0));
      AddRow(new Vector3(10, 5, 0), new Vector3(0, 5, 0), 5, new Vector3(5, 5, 0));
    }
  }
}
