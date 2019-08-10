using System;
using System.Collections.Generic;
using System.Linq;
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
      return GetFighters(count, powerlevel, null);
    }

    public static IEnumerable<IFighterStats> GetFighters(int count, int powerlevel, Guid? team)
    {
      for (int i = 0; i < count; i++)
      {
        yield return GetFighter(powerlevel, team);
      }
    }

    public static IFighterStats GetFighter(int powerlevel)
    {
      return GetFighter(powerlevel, null);
    }

    public static IFighterStats GetFighter(int powerlevel, Guid? team)
    {
      int offensive = (int)(powerlevel * 0.4);
      int defensive = (int)(powerlevel * 0.4);
      int utility = (int)(powerlevel * 0.2);

      offensive += powerlevel - (offensive + defensive + utility);

      return GetFighter(offensive, defensive, utility, team);
    }

    public static IEnumerable<IFighterStats> GetFighters(int count, int offensivePowerlevel, int defensivePowerlevel, int utilityPowerlevel)
    {
      return GetFighters(count, offensivePowerlevel, defensivePowerlevel, utilityPowerlevel, null);
    }

    public static IEnumerable<IFighterStats> GetFighters(int count, int offensivePowerlevel, int defensivePowerlevel, int utilityPowerlevel, Guid? team)
    {
      for (int i = 0; i < count; i++)
      {
        yield return GetFighter(offensivePowerlevel, defensivePowerlevel, utilityPowerlevel, team);
      }
    }

    public static IFighterStats GetFighter(int offensivePowerlevel, int defensivePowerlevel, int utilityPowerlevel)
    {
      return GetFighter(offensivePowerlevel, defensivePowerlevel, utilityPowerlevel, null);
    }

    public static IFighterStats GetFighter(int offensivePowerlevel, int defensivePowerlevel, int utilityPowerlevel, Guid? team)
    {
      var fighter = new GenericFighter()
      {
        Id = Guid.NewGuid(),
        Team = team,
      };

      SetValues(ref fighter, OffensiveProperties, offensivePowerlevel);
      SetValues(ref fighter, DefensiveProperties, defensivePowerlevel);
      SetValues(ref fighter, UtilityProperties, utilityPowerlevel);

      return fighter;
    }

    public static IEnumerable<IFighterStats> GetFighterTeams(int teamCount, int teamSize, int powerlevel)
    {
      for (int teamNumber = 0; teamNumber < teamCount; teamNumber++)
      {
        Guid team = Guid.NewGuid();
        for (int fighterNumber = 0; fighterNumber < teamSize; fighterNumber++)
        {
          yield return GetFighter(powerlevel, team);
        }
      }
    }

    private static void SetValues(ref GenericFighter fighter, IEnumerable<PropertyInfo> properties, int powerlevel)
    {
      var propertiesCount = properties.Count();

      while (powerlevel > 0)
      {
        foreach (var property in properties)
        {
          int max = (powerlevel + 1) / propertiesCount;
          if (max == 0)
          {
            max = 1;
          }

          int value = Random.Next(1, max);
          powerlevel -= value;
          if (powerlevel < 0)
          {
            break;
          }

          var current = (float)property.GetValue(fighter);

          property.SetValue(fighter, current + value);
        }
      }
    }
  }
}
