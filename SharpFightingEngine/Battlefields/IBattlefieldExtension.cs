namespace SharpFightingEngine.Battlefields
{
  public static class IBattlefieldExtension
  {
    public static IPosition GetCenter(this IBattlefield battlefield)
    {
      var centerVector = battlefield.CurrentBounds.High / battlefield.CurrentBounds.Low;
      var center = default(Position);
      center.Set(centerVector);

      return center;
    }
  }
}
