using System.Numerics;

namespace SharpFightingEngine.Test.Data.Battlefields
{
  public class InBoundsTheoryData : TheoryData
  {
    public InBoundsTheoryData()
    {
      AddRow(new Vector3(0, 0, 0), new Vector3(255, 255, 255), new Vector3(50, 50, 50), true);
      AddRow(new Vector3(0, 0, 0), new Vector3(255, 255, 255), new Vector3(0, 0, 0), true);
      AddRow(new Vector3(0, 0, 0), new Vector3(255, 255, 255), new Vector3(255, 255, 255), true);
      AddRow(new Vector3(0, 0, 0), new Vector3(255, 255, 255), new Vector3(0, 0, 256), false);
      AddRow(new Vector3(0, 0, 0), new Vector3(255, 255, 255), new Vector3(-1, 0, 0), false);
    }
  }
}
