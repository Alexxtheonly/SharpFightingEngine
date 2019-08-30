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
      AddRow(new Position(50, 48), new Position(50, 52), new Position(50, 55));
      AddRow(new Position(38.4104424F, 21.40334F), new Position(40.79111F, 19.5510216F), new Position(45.9577179F, 16.5004177F));
    }
  }
}
