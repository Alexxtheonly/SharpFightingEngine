using System;
using System.Numerics;

namespace SharpFightingEngine.Battlefields
{
  public interface IBounds
  {
    Guid Id { get; }

    Vector3 Low { get; }

    Vector3 High { get; }
  }
}
