using Newtonsoft.Json.Linq;
using WeatherApi.Models;

namespace WeatherApi.Services.Concretes
{
	public class WeatherService
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;

		public WeatherService(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
			_configuration = configuration;
		}
		public async Task<Report> GetWeather(double latitude, double longitude)
		{
			var apiKey = _configuration["OpenWeatherMap:ApiKey"];
			var unit = _configuration["OpenWeatherMap:Units"];
			var response = _httpClient.GetAsync(
				$"weather?lat={latitude}&lon={longitude}&appid={apiKey}&units={unit}").Result;

			var content = response.Content.ReadAsStringAsync().Result;
			var jsonResponse = JObject.Parse(content);

			return new Report
			{
				WeatherId = (int)jsonResponse["weather"][0]["id"],
				Main = (string)jsonResponse["weather"][0]["main"],
				Description = (string)jsonResponse["weather"][0]["description"],
				Icon = (string)jsonResponse["weather"][0]["icon"],
				Temp = (float)jsonResponse["main"]["temp"],
				FeelsLike = (float)jsonResponse["main"]["feels_like"],
				TempMin = (float)jsonResponse["main"]["temp_min"],
				TempMax = (float)jsonResponse["main"]["temp_max"],
				Pressure = (float)jsonResponse["main"]["pressure"],
				Humidity = (float)jsonResponse["main"]["humidity"],
				SeaLevel = jsonResponse["main"]["sea_level"] != null ? (float)jsonResponse["main"]["sea_level"] : 0,
				GroundLevel = jsonResponse["main"]["grnd_level"] != null ? (float)jsonResponse["main"]["grnd_level"] : 0,
				WindSpeed = (float)jsonResponse["wind"]["speed"],
				WindDegree = (float)jsonResponse["wind"]["deg"],
				WindGust = jsonResponse["wind"]["gust"] != null ? (float)jsonResponse["wind"]["gust"] : 0,
				Clouds = (float)jsonResponse["clouds"]["all"]
			};
		}
	}
}
