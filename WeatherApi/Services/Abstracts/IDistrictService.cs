using WeatherApi.Models;

namespace WeatherApi.Services.Abstracts
{
	public interface IDistrictService
	{
		public District Get(Func<District, bool>? func = null);
		public List<District> GetAll(Func<District,bool>? func=null);
		public void Upload(List<District> districts);
		public Task<List<District>> DeserializeAsync(IFormFile file);
	}
}
