using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Engines.Ticks
{
  public class EngineRoundTeamScoreTick : EngineTick, IEngineRoundScoreTick
  {
    private readonly EngineRoundTick roundTick;
    private readonly IEnumerable<IFighter> teamFighters;

    public EngineRoundTeamScoreTick(EngineRoundTick roundTick, IEnumerable<IFighter> teamFighters)
    {
      this.roundTick = roundTick;
      this.teamFighters = teamFighters;

      // it's the engines job to ensure that wrong teamFighters come in here.
      TeamId = teamFighters.First().Team.Value;

      Round = roundTick.Round;
      Health = teamFighters.Sum(o => Math.Max(0, o.Health));
      Energy = teamFighters.Sum(o => o.Energy);
    }

    public Guid TeamId { get; private set; }

    public int DamageDone => roundTick.Ticks
      .OfType<FighterAttackTick>()
      .Where(o => o.Hit)
      .Where(o => o.Fighter.Team == TeamId)
      .Sum(o => o.Damage);

    public int DamageTaken => roundTick.Ticks
      .OfType<FighterAttackTick>()
      .Where(o => o.Hit)
      .Where(o => o.Target.Team == TeamId)
      .Sum(o => o.Damage);

    public int Deaths => teamFighters.Count(o => o.Health <= 0);

    public float DistanceTraveled => roundTick.Ticks
      .OfType<FighterMoveTick>()
      .Where(o => o.Fighter.Team == TeamId)
      .Sum(o => o.Current.GetDistance(o.Next));

    public int Energy { get; private set; }

    public int EnergyUsed => roundTick.Ticks
      .OfType<FighterAttackTick>()
      .Where(o => o.Fighter.Team == TeamId)
      .Sum(o => o.Skill.Energy);

    public int Health { get; private set; }

    public int Kills => roundTick.Ticks
      .OfType<FighterAttackTick>()
      .Where(o => o.Hit)
      .GroupBy(o => o.Target)
      .Select(o => o.OrderByDescending(u => u.DateTime).First())
      .Where(o => o.Fighter.Team == TeamId)
      .Where(o => o.Target.Health <= 0)
      .Count();

    public int RestoredEnergy => roundTick.Ticks
      .OfType<FighterRegenerateEnergyTick>()
      .Where(o => o.Fighter.Team == TeamId)
      .Sum(o => o.RegeneratedEnergy);

    public int RestoredHealth => roundTick.Ticks
      .OfType<FighterRegenerateHealthTick>()
      .Where(o => o.Fighter.Team == TeamId)
      .Sum(o => o.HealthPointsRegenerated);

    public int Round { get; private set; }

    public override string ToString()
    {
      return $"Team {TeamId}\t{Health}\t{DamageTaken}\t{Energy}\t{EnergyUsed}\t{Kills}\t{Deaths}";
    }
  }
}
