using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Palermo.BlazorMvc;
using ProgrammingWithPalermo.ChurchBulletin.Core.Model;
using static System.Net.WebRequestMethods;

namespace UI.Maui.Pages;

[Route("/fetchdata")]
public class FetchDataController : ControllerComponentBase<FetchDataView>
{
    private WeatherForecast[]? _forecasts;
    [Inject] public HttpClient? Http { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Debug.Assert(Http != null, nameof(Http) + " != null");

        

        _forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
        View.Model = _forecasts;
    }
}