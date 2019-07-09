using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SharpFightingEngine.Skills.Test.General
{
  public class ISkillTest
  {
    [Fact]
    public void ShouldHaveUniqueSkillIds()
    {
      var skillTypes = AppDomain.CurrentDomain
        .GetAssemblies()
        .Where(assembly => !assembly.FullName.StartsWith("Microsoft.VisualStudio.TraceDataCollector")) // workaround for https://github.com/microsoft/vstest/issues/2008
        .SelectMany(o => o.GetTypes())
        .Where(o => o.GetInterfaces().Contains(typeof(ISkill)));

      var ids = new HashSet<Guid>();
      foreach (var type in skillTypes)
      {
        var skill = Assert.IsAssignableFrom<ISkill>(Activator.CreateInstance(type));
        Assert.DoesNotContain(skill.Id, ids);
        ids.Add(skill.Id);
      }

      Assert.Equal(skillTypes.Count(), ids.Count);
    }
  }
}
