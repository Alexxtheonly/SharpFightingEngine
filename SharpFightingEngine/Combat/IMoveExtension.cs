using System;
using System.Collections.Generic;
using System.Numerics;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Combat;
using SharpFightingEngine.Engines;
using SharpFightingEngine.Engines.Ticks;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Combat
{
  public static class IMoveExtension
  {
    public static EngineTick Handle(this IMove move, Dictionary<Guid, IFighterStats> fighters, EngineCalculationValues calculationValues)
    {
      var moveTick = new FighterMoveTick()
      {
        Fighter = move.Actor.AsStruct(),
        Current = move.Actor.GetPosition(),
      };

      if (move.HasEnoughSpeed(calculationValues))
      {
        fighters[move.Actor.Id].Set(move.NextPosition);

        moveTick.Next = move.Actor.GetPosition();
        return moveTick;
      }

      var distance = move.GetDistance();
      if (distance == 0)
      {
        moveTick.Next = move.Actor.GetPosition();
        return moveTick;
      }
      else
      {
        var startPosition = move.Actor.GetVector3();
        var endPosition = move.NextPosition.GetVector3();

        var newPosition = startPosition + (Vector3.Normalize(endPosition - startPosition) * move.Actor.Velocity(calculationValues));

        fighters[move.Actor.Id].Set(newPosition);

        moveTick.Next = move.Actor.GetPosition();
        return moveTick;
      }
    }

    public static float GetDistance(this IMove move)
    {
      return move.Actor.GetDistance(move.NextPosition);
    }

    public static bool HasEnoughSpeed(this IMove move, EngineCalculationValues calculationValues)
    {
      return move.Actor.Velocity(calculationValues) >= Math.Abs(move.GetDistance());
    }
  }
}
