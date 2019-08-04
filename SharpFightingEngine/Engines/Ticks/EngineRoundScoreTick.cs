using System;
using System.Linq;
using SharpFightingEngine.Battlefields;
using SharpFightingEngine.Fighters;

namespace SharpFightingEngine.Engines.Ticks
{
  public class EngineRoundScoreTick : EngineTick, IEngineRoundScoreTick
  {
    private readonly EngineRoundTick roundTick;

    public EngineRoundScoreTick(EngineRoundTick roundTick, IFighterStats fighter)
    {
      this.roundTick = roundTick;

      FighterId = fighter.Id;
      Powerlevel = (int)fighter.PowerLevel();
      Round = roundTick.Round;
      Health = fighter.Health;
      Energy = fighter.Energy;
    }

    public Guid FighterId { get; private set; }

    public int Powerlevel { get; private set; }

    public int Round { get; private set; }

    public int Health { get; private set; }

    public int Energy { get; private set; }

    public int EnergyUsed => roundTick.Ticks
      .OfType<FighterAttackTick>()
      .Where(o => o.Fighter.Id == FighterId)
      .Sum(o => o.Skill.Energy);

    public int DamageTaken => roundTick.Ticks
      .OfType<FighterAttackTick>()
      .Where(o => o.Hit)
      .Where(o => o.Target.Id == FighterId)
      .Sum(o => o.Damage);

    public int DamageDone => roundTick.Ticks
      .OfType<FighterAttackTick>()
      .Where(o => o.Hit)
      .Where(o => o.Fighter.Id == FighterId)
      .Sum(o => o.Damage);

    public int Kills => roundTick.Ticks
      .OfType<EngineFighterDiedTick>()
      .SelectMany(o => roundTick.Ticks.OfType<FighterAttackTick>().Where(u => u.Target.Id == o.Fighter.Id))
      .Where(o => o.Fighter.Id == FighterId)
      .Select(o => o.Fighter.Id)
      .Distinct()
      .Count();

    public int RestoredHealth => roundTick.Ticks
      .OfType<FighterRegenerateHealthTick>()
      .Where(o => o.Fighter.Id == FighterId)
      .Sum(o => o.HealthPointsRegenerated);

    public int RestoredEnergy => roundTick.Ticks
      .OfType<FighterRegenerateEnergyTick>()
      .Where(o => o.Fighter.Id == FighterId)
      .Sum(o => o.RegeneratedEnergy);

    public int Deaths => Health <= 0 ? 1 : 0;

    public float DistanceTraveled => roundTick.Ticks
      .OfType<FighterMoveTick>()
      .Where(o => o.Fighter.Id == FighterId)
      .Sum(o => o.Current.GetDistance(o.Next));

    public override string ToString()
    {
      return $"Fighter {FighterId}\t{Health}\t{DamageTaken}\t{Energy}\t{EnergyUsed}\t{Kills}\t{Deaths}";
    }
  }
}
