using System;

namespace SharpFightingEngine.Skills
{
  public interface ISkill
  {
    Guid Id { get; }

    string Name { get; }

    int Damage { get; }

    float Range { get; }

    int Energy { get; }
  }
}
