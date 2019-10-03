using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SharpFightingEngine.Skills;
using SharpFightingEngine.Skills.General;
using SharpFightingEngine.Utilities;

namespace SharpFightingEngine.Fighters.Factories
{
  public class FighterFactory
  {
    private static readonly IEnumerable<PropertyInfo> OffensiveProperties = typeof(IOffensiveStats).GetProperties();

    private static readonly IEnumerable<PropertyInfo> DefensiveProperties = typeof(IDefensiveStats).GetProperties();

    private static readonly IEnumerable<PropertyInfo> UtilityProperties = typeof(IUtilityStats).GetProperties();

    private static readonly Random Random = new Random();

    private static readonly IEnumerable<ISkill> Skills = typeof(DamageSkillBase)
      .Assembly
      .GetTypes()
      .Where(o => o.GetInterfaces().Contains(typeof(ISkill)) && !o.IsAbstract)
      .Select(o => (ISkill)Activator.CreateInstance(o));

    public static IEnumerable<IFighterStats> GetFighters(int count, Action<IStats> customStats)
    {
      for (int i = 0; i < count; i++)
      {
        var fighter = new AdvancedFighter()
        {
          Id = Guid.NewGuid(),
        };

        AddSkills(fighter);

        customStats.Invoke(fighter.Stats);

        yield return fighter;
      }
    }

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
      var fighter = new AdvancedFighter()
      {
        Id = Guid.NewGuid(),
        Team = team,
      };

      AddSkills(fighter);

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

    public static void AddSkills(FighterBase fighter)
    {
      var defaultSkill = Skills.Where(o => o.Cooldown == 0).GetRandom();
      var others = Skills.Shuffle().Take(3);
      var heal = Skills.OfType<IHealSkill>().GetRandom();

      fighter.Skills = others.Union(new ISkill[]
      {
        defaultSkill,
        heal,
      });
    }

    private static void SetValues(ref AdvancedFighter fighter, IEnumerable<PropertyInfo> properties, int powerlevel)
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

          var current = (int)property.GetValue(fighter.Stats);

          property.SetValue(fighter.Stats, current + value);
        }
      }
    }
  }
}
