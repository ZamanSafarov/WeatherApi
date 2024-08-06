namespace WeatherApi.Exceptions
{
	public class EntityAlreadyExsist : Exception
	{
		public EntityAlreadyExsist(string? message) : base(message)
		{
		}
	}
}
