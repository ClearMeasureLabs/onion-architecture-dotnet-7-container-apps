using Newtonsoft.Json;
using NUnit.Framework;
using ProgrammingWithPalermo.ChurchBulletin.Core.Model;
using Shouldly;

namespace ProgrammingWithPalermo.ChurchBulletin.UnitTests;

[TestFixture]
public class UpperCaseStringTester
{
    [Test]
    public void ShouldBeAssignableFromString()
    {
        UpperCaseString? s = "abc";
        string? s2 = s;

        string? result = s2;
        string? result2 = s.ToString();

        result2.ShouldBe(result);
    }

    [Test]
    public void CanSerialize()
    {
        UpperCaseString s = "abc";
        string serialized = JsonConvert.SerializeObject(s);
        Console.WriteLine(serialized);
        UpperCaseString deserializeObject = JsonConvert.DeserializeObject<UpperCaseString>(serialized)!;
        deserializeObject.ToString().ShouldBe("ABC");
    }
}