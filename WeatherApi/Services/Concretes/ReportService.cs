using System.Linq;
using WeatherApi.DAL;
using WeatherApi.DTO;
using WeatherApi.Models;
using WeatherApi.Services.Abstracts;

namespace WeatherApi.Services.Concretes
{
	public class ReportService: IReportService
	{
		private readonly AppDbContext _context;
		public ReportService(AppDbContext appDbContext)
		{
			_context = appDbContext;
		}
		public List<Report> FilterReport(Pagination pagination,ReportFilterDTO filter)
		{
			var query = _context.Reports.AsQueryable();

			if (filter.WeatherId.HasValue)
				query = query.Where(r => r.WeatherId == filter.WeatherId.Value);

			if (!string.IsNullOrEmpty(filter.Main))
				query = query.Where(r => r.Main == filter.Main);

			if (!string.IsNullOrEmpty(filter.Description))
				query = query.Where(r => r.Description == filter.Description);


			if (filter.FeelsLikeMin.HasValue)
				query = query.Where(r => r.FeelsLike >= filter.FeelsLikeMin.Value);

			if (filter.FeelsLikeMax.HasValue)
				query = query.Where(r => r.FeelsLike <= filter.FeelsLikeMax.Value);

			if (filter.PressureMin.HasValue)
				query = query.Where(r => r.Pressure >= filter.PressureMin.Value);

			if (filter.PressureMax.HasValue)
				query = query.Where(r => r.Pressure <= filter.PressureMax.Value);
			if (filter.TempMin.HasValue)
				query = query.Where(r => r.Temp >= filter.TempMin.Value);

			if (filter.TempMax.HasValue)
				query = query.Where(r => r.Temp <= filter.TempMax.Value);
			if (filter.HumidityMin.HasValue)
				query = query.Where(r => r.Humidity >= filter.HumidityMin.Value);

			if (filter.HumidityMax.HasValue)
				query = query.Where(r => r.Humidity <= filter.HumidityMax.Value);

			if (filter.WindSpeedMin.HasValue)
				query = query.Where(r => r.WindSpeed >= filter.WindSpeedMin.Value);

			if (filter.WindSpeedMax.HasValue)
				query = query.Where(r => r.WindSpeed <= filter.WindSpeedMax.Value);

			if (filter.WindDegreeMin.HasValue)
				query = query.Where(r => r.WindDegree >= filter.WindDegreeMin.Value);

			if (filter.WindDegreeMax.HasValue)
				query = query.Where(r => r.WindDegree <= filter.WindDegreeMax.Value);

			if (filter.CloudsMin.HasValue)
				query = query.Where(r => r.Clouds >= filter.CloudsMin.Value);

			if (filter.CloudsMax.HasValue)
				query = query.Where(r => r.Clouds <= filter.CloudsMax.Value);

			if (filter.DisctrictId.HasValue)
				query = query.Where(r => r.DisctrictId == filter.DisctrictId.Value);

			if (filter.DateTimeStart.HasValue)
				query = query.Where(r => r.DateTime >= filter.DateTimeStart.Value);

			if (filter.DateTimeEnd.HasValue)
				query = query.Where(r => r.DateTime <= filter.DateTimeEnd.Value);

			var paginatedReports = query
				.Skip((pagination.PageNumber - 1) * pagination.PageSize)
				.Take(pagination.PageSize)
				.ToList();

			return paginatedReports;
		}
	
		public void AddRep(Report model, int districtId)
		{
			if (!_context.Reports.ToList().Any(x=>x.Id == districtId))
			{
				var reportDis = _context.Reports.Where(x => x.DisctrictId == districtId).FirstOrDefault();

				reportDis.SeaLevel = model.SeaLevel;
				reportDis.FeelsLike = model.FeelsLike;
			}
			else
			{
				model.DisctrictId = districtId;
				model.DateTime = DateTime.Now.ToUniversalTime();
				_context.Reports.Add(model);
			}
			
			_context.SaveChanges();

		}

		
	}
}
