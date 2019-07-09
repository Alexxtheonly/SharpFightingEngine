using System;

namespace SharpFightingEngine.Engines.Ticks
{
  public abstract class EngineTick
  {
    public EngineTick()
    {
      DateTime = DateTimeOffset.Now;
    }

    public DateTimeOffset DateTime { get; private set; }
  }
}
