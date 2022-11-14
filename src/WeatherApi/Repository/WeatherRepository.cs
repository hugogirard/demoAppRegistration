using Newtonsoft.Json;
using StackExchange.Redis;

namespace WeatherApi.Repository;

public class WeatherRepository : IWeatherRepository
{
    private readonly ILogger<WeatherRepository> _logger;
    private readonly ConnectionMultiplexer _connection;
    private readonly IDatabase _database;
    private readonly bool _cacheAvailable;

    public WeatherRepository(IConfiguration configuration, ILogger<WeatherRepository> logger)
    {

        _logger = logger;

        try
        {
#if DEBUG        
            _connection = ConnectionMultiplexer.Connect("localhost:6379");
#else         
            _connection = ConnectionMultiplexer.Connect(configuration["RedisCnxString"]);
#endif        
            _database = _connection.GetDatabase();

            _cacheAvailable = true;

        }
        catch (System.Exception ex)
        {
            _logger.LogError("Cannot create connection", ex.Message);
        }
    }

    public async Task<City> GetWeatherAsync(City city)
    {
        try
        {
            string key = $"{city.Name}-{city.Country}";

            if (!_cacheAvailable)
                return null;

            _logger.LogDebug($"Get value len {key} from cache");

            var value = await _database.StringGetAsync(new RedisKey(key));

            if (value.HasValue)
            {
                var returnedCity = JsonConvert.DeserializeObject<City>(value.ToString());
                returnedCity.ValueFromCache = true;
                return returnedCity;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("Cannot get value from cache", ex.Message);
            return null;
        }
    }

    public async Task SetSequenceValue(City city)
    {
        try
        {

            string key = $"{city.Name}-{city.Country}";

            if (!_cacheAvailable)
                return;

            await _database.StringSetAsync(new RedisKey(key),
                                           new RedisValue(JsonConvert.SerializeObject(city)));
        }
        catch (System.Exception ex)
        {
            _logger.LogError("Cannot set value in cache", ex.Message);
        }
    }

    public async Task ClearCacheAsync()
    {
        try
        {
            var endpoints = _connection.GetEndPoints();
            var server = _connection.GetServer(endpoints.First());

            var keys = server.Keys();

            foreach (var key in keys)
            {
                await _database.KeyDeleteAsync(key);
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError("Cannot set value in cache", ex.Message);
        }
    }
}   