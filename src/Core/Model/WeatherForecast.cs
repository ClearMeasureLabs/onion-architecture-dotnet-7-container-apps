namespace ProgrammingWithPalermo.ChurchBulletin.Core.Model
{
    /// <summary>
    /// Template class from sample app - moved from UI.Shared
    /// </summary>
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public UpperCaseString? Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}