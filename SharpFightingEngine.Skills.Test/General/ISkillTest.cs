using System;
using System.Collections.Generic;
using System.Linq;
using SharpFightingEngine.Skills.General;
using Xunit;

namespace SharpFightingEngine.Skills.Test.General
{
  public class ISkillTest
  {
    [Fact]
    public void ShouldHaveUniqueSkillIds()
    {
      var ids = new HashSet<Guid>();
      var skills = GetSkills().ToList();
      foreach (var skill in skills)
      {
        Assert.DoesNotContain(ids, o => o == skill.Id);

        ids.Add(skill.Id);
      }

      Assert.Equal(skills.Count, ids.Count);
    }

    [Fact]
    public void ShouldHaveCorrectDamage()
    {
      foreach (var skill in GetSkills())
      {
        Assert.True(skill.DamageLow <= skill.DamageHigh);
        Assert.InRange(skill.Damage, skill.DamageLow, skill.DamageHigh);
      }
    }

    private IEnumerable<DamageSkillBase> GetSkills()
    {
      var skillTypes = AppDomain.CurrentDomain
        .GetAssemblies()
        .Where(assembly => !assembly.FullName.StartsWith("Microsoft.VisualStudio.TraceDataCollector")) // workaround for https://github.com/microsoft/vstest/issues/2008
        .SelectMany(o => o.GetTypes())
        .Where(o => o.BaseType == typeof(DamageSkillBase));

      foreach (var type in skillTypes)
      {
        var skill = Assert.IsAssignableFrom<DamageSkillBase>(Activator.CreateInstance(type));

        yield return skill;
      }
    }
  }
}
