using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApi.DAL;
using WeatherApi.DTO;
using WeatherApi.Models;
using WeatherApi.Services.Abstracts;
using WeatherApi.Services.Concretes;

namespace WeatherApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ReportController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IReportService _reportService;
		private readonly WeatherService _weatherService;

		public ReportController(AppDbContext context, IReportService reportService, WeatherService weatherService) 
		{
			_context = context;
			_reportService = reportService;
			_weatherService = weatherService;
		}

		[HttpGet]
		public ActionResult<List<Report>> GetReports([FromQuery] ReportFilterDTO filter, [FromQuery] Pagination pagination)
		{
			return Ok(_reportService.FilterReport(pagination,filter));
		}
		[HttpPost]
		public IActionResult WeatherCheck([FromBody] District model)
		{
			var weather = _weatherService.GetWeather(model.Latitude, model.Longitude);

			_reportService.AddRep(weather, model.Id);

			return Ok(weather);
		}

	}
}
