namespace PoliFemoBackend;

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public decimal TemperatureC { get; set; }

    public decimal TemperatureF => 32 + (decimal)(TemperatureC / 0.5556m);

    public string? Summary { get; set; }
}