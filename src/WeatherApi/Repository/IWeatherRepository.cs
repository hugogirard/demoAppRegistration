namespace WeatherApi.Repository;

public interface IWeatherRepository
{
    Task ClearCacheAsync();
    Task<City> GetWeatherAsync(City city);
    Task SetSequenceValue(City city);
}
