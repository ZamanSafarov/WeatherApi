using System;
using System.Xml;
using System.Xml.Linq;
using WeatherApi.DAL;
using WeatherApi.Exceptions;
using WeatherApi.Models;
using WeatherApi.Services.Abstracts;

namespace WeatherApi.Services.Concretes
{
	public class DistrictService : IDistrictService
	{
		private readonly AppDbContext _context;
        public DistrictService(AppDbContext context)
        {
			_context= context;

		}
		//public void UploadDistrictsFromXml(string xmlFilePath)
		//{
		//	var xmlDoc = new XmlDocument();
		//	xmlDoc.Load(xmlFilePath);

		//	var nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
		//	nsManager.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

		//	var rows = xmlDoc.SelectNodes("//ss:Row", nsManager);

		//	for (int i = 1; i < rows.Count; i++) // Skipping header row
		//	{
		//		var cells = rows[i].SelectNodes("ss:Cell/ss:Data", nsManager);
		//		var id = int.Parse(cells[0].InnerText);
		//		var title = cells[1].InnerText;
		//		var latitude = double.Parse(cells[2].InnerText);
		//		var longitude = double.Parse(cells[3].InnerText);

		//		var existingDistrict = _context.Districts.FirstOrDefault(d => d.Id == id);
		//		if (existingDistrict != null)
		//		{
		//			existingDistrict.Title = title;
		//			existingDistrict.Latitude = latitude;
		//			existingDistrict.Longitude = longitude;
		//		}
		//		else
		//		{
		//			var newDistrict = new District
		//			{
		//				Id = id,
		//				Title = title,
		//				Latitude = latitude,
		//				Longitude = longitude
		//			};
		//			_context.Districts.Add(newDistrict);
		//		}
		//	}

		//	_context.SaveChanges();
		//}
		public async Task<List<District>> DeserializeAsync(IFormFile file)
		{
			var districts = new List<District>();
			using (var stream = new MemoryStream())
			{
				await file.CopyToAsync(stream);
				stream.Position = 0;

				var doc = XDocument.Load(stream);
				XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";

				foreach (var row in doc.Descendants(ss + "Row").Skip(1))
				{
					var cells = row.Elements(ss + "Cell").ToList();
					if (cells.Count >= 4) 
					{
						var district = new District
						{
							Title = cells[1].Element(ss + "Data").Value,
							Latitude = float.Parse(cells[2].Element(ss + "Data").Value),
							Longitude = float.Parse(cells[3].Element(ss + "Data").Value)
						};
						districts.Add(district);
					}
				}
			}
			return districts;
		}

		public District Get(Func<District, bool>? func = null)
		{

			return func == null ? _context.Districts.FirstOrDefault() : _context.Districts.Where(func).FirstOrDefault();
		}

		public List<District> GetAll(Func<District, bool>? func = null)
		{

			return func == null ? _context.Districts.ToList() : _context.Districts.Where(func).ToList();
		}

		public void Upload(List<District> districts)
		{
			var dbDistricts = _context.Districts.ToList();

			foreach (var item in districts)
			{
				if (!dbDistricts.Any(x=>x.Title ==item.Title))
				{
					_context.Add(item);
					_context.SaveChanges();
				}
				else
				{
					throw new EntityAlreadyExsist("District already exsist");
				}
			
			}
		}
	}
}
