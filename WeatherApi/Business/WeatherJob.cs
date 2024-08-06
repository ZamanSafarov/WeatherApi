using WeatherApi.Services.Abstracts;
using WeatherApi.Services.Concretes;

namespace WeatherApi.Business
{
    public class WeatherJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public WeatherJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        var weatherService = scope.ServiceProvider.GetRequiredService<WeatherService>();
                        var districtService = scope.ServiceProvider.GetRequiredService<IDistrictService>();
                        var repService = scope.ServiceProvider.GetRequiredService<IReportService>();

                        var districts = districtService.GetAll();

                        foreach (var district in districts)
                        {
                            var weather = weatherService.GetWeather(district.Latitude, district.Longitude);
                            repService.AddRep(weather, district.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (use your preferred logging framework)
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken); // Adjust the delay as needed
            }
        }
    }
}
