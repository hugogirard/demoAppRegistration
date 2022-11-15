using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Microsoft.Extensions.Caching.StackExchangeRedis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWeatherRepository, WeatherRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(o => 
{
    o.RoutePrefix = string.Empty;
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API V1");
});

app.UseHttpsRedirection();

var readWeatherScope = app.Configuration["AzureAd:Scopes:ReadWeatherScope"] ?? "";
var readWeatherMarsScope = app.Configuration["AzureAd:Scopes:ReadWeatherMars"] ?? "";
var deleteCacheScope = app.Configuration["AzureAd:Scopes:DeleteCacheScope"] ?? "";

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/api", async (IWeatherRepository weatherRepository, HttpContext httpContext, string cityName, string country) =>
{
    httpContext.VerifyUserHasAnyAcceptedScope(readWeatherScope);
    
    var city = new City { Name = cityName, Country = country };

    var cachedValue = await weatherRepository.GetWeatherAsync(city);

    if (cachedValue != null)           
        return cachedValue;
        
    var forecasts =  Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        (
                            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            Random.Shared.Next(-20, 55),
                            summaries[Random.Shared.Next(summaries.Length)]
                        ))
                        .ToArray();

    await weatherRepository.SaveForecastsCityAsync(city,forecasts);

    return forecasts;
})
.WithName("GetWeatherForecast")
.WithOpenApi()
.RequireAuthorization();

app.MapGet("/api/getWeatherFromMars", async (IWeatherRepository weatherRepository, HttpContext httpContext) =>
{
    httpContext.ValidateAppRole(new string[] { "SecretAgent" });
    httpContext.VerifyUserHasAnyAcceptedScope(readWeatherScope);
    
    var city = new City { Name = "Mars", Country = "Mars" };

    var cachedValue = await weatherRepository.GetWeatherAsync(city);

    if (cachedValue != null)
        return cachedValue;

    var forecasts =  Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        (
                            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            Random.Shared.Next(-140, 21),
                            summaries[Random.Shared.Next(summaries.Length)]
                        ))
                        .ToArray();

    await weatherRepository.SaveForecastsCityAsync(city,forecasts);

    return forecasts;
})
.WithName("GetWeatherForecastFromMars")
.WithOpenApi()
.RequireAuthorization();

app.MapDelete("/api/deleteCache", async (IWeatherRepository weatherRepository, HttpContext httpContext) =>
{
    httpContext.ValidateAppRole(new string[] { "Admin.Weather.App" });
    httpContext.VerifyUserHasAnyAcceptedScope(deleteCacheScope);
    
    await weatherRepository.ClearCacheAsync();
    
})
.WithName("DeleteCache")
.WithOpenApi()
.RequireAuthorization();

app.Run();

