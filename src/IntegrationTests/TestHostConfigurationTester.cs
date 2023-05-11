using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Shouldly;

namespace ProgrammingWithPalermo.ChurchBulletin.IntegrationTests;

[TestFixture]
public class TestHostConfigurationTester
{
    [Test]
    public void ShouldReadVariableFromConfigFile()
    {
        IConfiguration config = TestHost.GetRequiredService<IConfiguration>();
        string? key = config.GetValue<string>("ConnectionStrings:SqlConnectionString");
        key.ShouldNotBeNullOrEmpty();
        Console.WriteLine(key);
    }

    [Test]
    public void ShouldReadVariableFromEnvironmentVariable()
    {
        string keyName = "ConnectionStrings:TestConnectionString";
        IConfiguration config = TestHost.GetRequiredService<IConfiguration>();
        config.GetValue<string>(keyName).ShouldBeNullOrEmpty();

        Environment.SetEnvironmentVariable(keyName, "test value", EnvironmentVariableTarget.Process);
        string? foundVariable = Environment.GetEnvironmentVariable(keyName);
        foundVariable.ShouldBe("test value");

        config = TestHost.GetRequiredService<IConfiguration>();
        (config as IConfigurationRoot)?.Reload();
        string? key = config.GetValue<string>(keyName);
        key.ShouldBe("test value");

        config.GetConnectionString("TestConnectionString").ShouldBe("test value");

    }
}