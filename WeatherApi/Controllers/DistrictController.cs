using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Exceptions;
using WeatherApi.Models;
using WeatherApi.Services;
using WeatherApi.Services.Abstracts;

namespace WeatherApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DistrictController : ControllerBase
	{
		private readonly IDistrictService _service;
        public DistrictController(IDistrictService service)
        {
            _service = service;
        }

		[HttpGet("GETALL")]
		public ActionResult<List<District>> GetDistricts()
		{
			var districts = _service.GetAll();
			return Ok(districts);
		}
		[HttpGet]
		public ActionResult<District> GetDistrict()
		{
			var district = _service.Get();
			return Ok(district);
		}
		[HttpPost("Upload")]
		public async Task<ActionResult<List<District>>> UploadDistrictAsync(IFormFile file)
		{
			var district = await _service.DeserializeAsync(file);
			try
			{
				_service.Upload(district);
			}
			catch (EntityAlreadyExsist msg)
			{
				return BadRequest(msg);
			}
			catch (Exception)
			{
				return BadRequest();
			}
			return Ok(district);
		}
	}
}
