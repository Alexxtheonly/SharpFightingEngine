using System;
using System.Collections.Generic;

namespace SharpFightingEngine.Fighters.Intelligence
{
  public class EnemyIntel
  {
    public Guid Id { get; set; }

    public Guid? LastTarget { get; set; }

    public int DamageTaken { get; set; }

    public int DamageDealt { get; set; }

    public float HealthPercent { get; set; }

    public int? LastRoundHealSkillUsed { get; set; }

    public bool IsStunned { get; set; }

    public bool IsHealingReduced { get; set; }

    public bool IsInRange { get; set; }

    public IEnumerable<Guid> LastAttackers { get; set; } = new List<Guid>();

    public int OtherFightersNearby { get; set; }
  }
}
