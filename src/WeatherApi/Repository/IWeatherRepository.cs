namespace WeatherApi.Repository;

public interface IWeatherRepository
{
    Task ClearCacheAsync();
    Task<WeatherForecast[]> GetWeatherAsync(City city);
    Task SaveForecastsCityAsync(City city, WeatherForecast[] weatherForecasts);
}
