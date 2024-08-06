using Microsoft.EntityFrameworkCore;
using WeatherApi.Models;

namespace WeatherApi.DAL
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions options):base(options)
		{
		}

		public DbSet<District> Districts { get; set; }
		public DbSet<Report> Reports { get; set; }
	}
}
