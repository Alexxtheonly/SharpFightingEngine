using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Test.Data;

namespace SharpFightingEngine.Fighters.Test.Data.Algorithms.Pathfinders
{
  public class PathFinderTheoryData : TheoryData
  {
    public PathFinderTheoryData()
    {
      AddRow(new Position(57.9350471F, 56.9235649F), new Position(50.3793526F, 55.40332F), new Position(57.3367462F, 58.3573265F));
      AddRow(new Position(44, 44), new Position(42, 44), new Position(46, 46));
    }
  }
}
