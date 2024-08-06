using WeatherApi.DTO;
using WeatherApi.Models;

namespace WeatherApi.Services.Abstracts
{
	public interface IReportService
	{
		public List<Report> FilterReport( Pagination pagination,ReportFilterDTO filter);
		public void AddRep(Report model, int districtId);
	}
}
