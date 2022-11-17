using Microsoft.AspNetCore.Mvc;

namespace ProgrammingWithPalermo.ChurchBulletin.UI.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VersionController : ControllerBase
{
    [HttpGet]
    public string? Get()
    {
        string? version = GetType().Assembly.GetName().Version?.ToString();
        return version;
    }
}