namespace SharpFightingEngine.Engines.Ticks
{
  public class FighterSpawnTick : FighterTick
  {
    public override string ToString()
    {
      return $"{base.ToString()} spawned with {Fighter.Health} health";
    }
  }
}
