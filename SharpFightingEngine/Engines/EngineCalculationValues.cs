namespace SharpFightingEngine.Engines
{
  public class EngineCalculationValues
  {
    public float HealthRegenerationFactor { get; set; } = 0.4F;

    public float EnergyRegenerationFactor { get; set; } = 0.33F;

    public float AttackPowerFactor { get; set; } = 2.2F;

    public float ArmorDefenseFactor { get; set; } = 1.2F;

    public float VisualRangeFactor { get; set; } = 4F;

    public float CriticalHitChanceFactor { get; set; } = 0.4F;

    public float CriticalHitDamageFactor { get; set; } = 2.5F;

    public float AccuracyFactor { get; set; } = 1.2F;

    public float AgilityFactor { get; set; } = 0.5F;

    public float VitalityFactor { get; set; } = 6F;

    public float SpeedFactor { get; set; } = 1;

    public float StaminaFactor { get; set; } = 2.3F;
  }
}
