using System.Numerics;
using SharpFightingEngine.Battlefields;

namespace SharpFightingEngine.Test.Data.Battlefields
{
  public class KnockbackTheoryData : TheoryData
  {
    public KnockbackTheoryData()
    {
      AddRow(new Vector2(0, 0).GetPosition(), new Vector2(2, 0).GetPosition(), 3, new Vector2(5, 0).GetPosition());
      AddRow(new Vector2(8, 0).GetPosition(), new Vector2(7, 0).GetPosition(), 3, new Vector2(4, 0).GetPosition());
    }
  }
}
