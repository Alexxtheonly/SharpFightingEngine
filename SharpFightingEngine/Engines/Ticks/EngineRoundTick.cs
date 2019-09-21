using System;
using System.Collections.Generic;
using System.Text;

namespace SharpFightingEngine.Engines.Ticks
{
  public class EngineRoundTick : EngineTick
  {
    public int Round { get; set; }

    public List<EngineTick> Ticks { get; private set; } = new List<EngineTick>();

    public override string ToString()
    {
      var stringbuilder = new StringBuilder();
      stringbuilder.AppendLine($"{nameof(Round)} {Round}");
      stringbuilder.AppendLine($"\t{string.Join($"{Environment.NewLine}\t", Ticks)}");

      return stringbuilder.ToString();
    }
  }
}
