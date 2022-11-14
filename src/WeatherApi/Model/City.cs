namespace WeatherApi.Model;

public class City
{
    public string Name { get; set; }
    public string Country { get; set; }

    public bool ValueFromCache { get; set; }
}
