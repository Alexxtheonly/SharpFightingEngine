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

    public EngineRoundTeamScoreTick(EngineRoundTick roundTick, IEnumerable<IFighterStats> teamFighters)
    {
      this.roundTick = roundTick;
      this.teamFighters = teamFighters
        .Select(o => o.AsStruct())
        .ToList();

      // The engine ensures that only fighters of the same team are passed to this class.
      TeamId = teamFighters.First().Team.Value;

      Powerlevel = (int)teamFighters.Sum(o => o.Stats.PowerLevel());
      Round = roundTick.Round;
      Health = teamFighters.Sum(o => Math.Max(0, o.Health));
      Energy = teamFighters.Sum(o => o.Energy);
    }

    public Guid TeamId { get; private set; }

    public int Powerlevel { get; private set; }

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
      .OfType<EngineFighterDiedTick>()
      .SelectMany(o => roundTick.Ticks.OfType<FighterAttackTick>().Where(u => u.Target.Id == o.Fighter.Id))
      .Where(o => o.Fighter.Team == TeamId)
      .Select(o => o.Fighter.Team)
      .Distinct()
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
