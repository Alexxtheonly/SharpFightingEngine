using System;
using System.Collections.Generic;
using System.Text;

namespace SharpFightingEngine.Engines.Ticks
{
  public class EngineRoundTick : EngineTick
  {
    public int Round { get; set; }

    public List<EngineTick> Ticks { get; private set; } = new List<EngineTick>();

    public ICollection<IEngineRoundScoreTick> ScoreTick { get; private set; } = new List<IEngineRoundScoreTick>();

    public override string ToString()
    {
      var stringbuilder = new StringBuilder();
      stringbuilder.AppendLine($"{nameof(Round)} {Round}");
      stringbuilder.AppendLine($"\t{string.Join($"{Environment.NewLine}\t", Ticks)}");
      stringbuilder.AppendLine();
      stringbuilder.AppendLine($"\t{string.Join($"{Environment.NewLine}\t", ScoreTick)}");

      return stringbuilder.ToString();
    }
  }
}
