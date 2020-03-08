using System.Numerics;
using SharpFightingEngine.Battlefields;

namespace SharpFightingEngine.Test.Data.Battlefields
{
  public class PullTheoryData : TheoryData
  {
    public PullTheoryData()
    {
      AddRow(new Vector2(0, 0).GetPosition(), new Vector2(5, 0).GetPosition(), 10, new Vector2(1, 0).GetPosition());
      AddRow(new Vector2(5, 0).GetPosition(), new Vector2(0, 0).GetPosition(), 10, new Vector2(4, 0).GetPosition());
      AddRow(new Vector2(0, 0).GetPosition(), new Vector2(14, 0).GetPosition(), 10, new Vector2(4, 0).GetPosition());
      AddRow(new Vector2(50, 50).GetPosition(), new Vector2(50, 50).GetPosition(), 14, new Vector2(50, 50).GetPosition());
    }
  }
}
