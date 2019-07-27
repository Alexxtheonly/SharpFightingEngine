using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharpFightingEngine.Fighters.Factories
{
  public class FighterFactory
  {
    private static readonly IEnumerable<PropertyInfo> OffensiveProperties = typeof(IOffensiveStats).GetProperties();

    private static readonly IEnumerable<PropertyInfo> DefensiveProperties = typeof(IDefensiveStats).GetProperties();

    private static readonly IEnumerable<PropertyInfo> UtilityProperties = typeof(IUtilityStats).GetProperties();

    private static readonly Random Random = new Random();

    public static IEnumerable<IFighterStats> GetFighters(int count, int powerlevel)
    {
      for (int i = 0; i < count; i++)
      {
        yield return GetFighter(powerlevel);
      }
    }

    public static IFighterStats GetFighter(int powerlevel)
    {
      int offensive = (int)(powerlevel * 0.4);
      int defensive = (int)(powerlevel * 0.4);
      int utility = (int)(powerlevel * 0.2);

      offensive += powerlevel - (offensive + defensive + utility);

      return GetFighter(offensive, defensive, utility);
    }

    public static IEnumerable<IFighterStats> GetFighters(int count, int offensivePowerlevel, int defensivePowerlevel, int utilityPowerlevel)
    {
      for (int i = 0; i < count; i++)
      {
        yield return GetFighter(offensivePowerlevel, defensivePowerlevel, utilityPowerlevel);
      }
    }

    public static IFighterStats GetFighter(int offensivePowerlevel, int defensivePowerlevel, int utilityPowerlevel)
    {
      var fighter = new GenericFighter()
      {
        Id = Guid.NewGuid(),
      };

      SetValues(ref fighter, OffensiveProperties, offensivePowerlevel);
      SetValues(ref fighter, DefensiveProperties, defensivePowerlevel);
      SetValues(ref fighter, UtilityProperties, utilityPowerlevel);

      return fighter;
    }

    private static void SetValues(ref GenericFighter fighter, IEnumerable<PropertyInfo> properties, int powerlevel)
    {
      while (powerlevel > 0)
      {
        foreach (var property in properties)
        {
          int value = Random.Next(1, powerlevel + 1);
          powerlevel -= value;
          if (powerlevel < 0)
          {
            break;
          }

          property.SetValue(fighter, value);
        }
      }
    }
  }
}
