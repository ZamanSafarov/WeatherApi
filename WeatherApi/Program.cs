
using Microsoft.EntityFrameworkCore;
using WeatherApi.Business;
using WeatherApi.DAL;
using WeatherApi.Services;
using WeatherApi.Services.Abstracts;
using WeatherApi.Services.Concretes;

namespace WeatherApi
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<AppDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("cString")));

			builder.Services.AddHttpClient();

			builder.Services.AddScoped<IDistrictService, DistrictService>();
			builder.Services.AddScoped<IReportService, ReportService>();
			builder.Services.AddScoped<WeatherService>();
			builder.Services.AddHostedService<WeatherJob>();
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
