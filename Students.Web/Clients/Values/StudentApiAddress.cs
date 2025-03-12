namespace Students.Web.Clients.Values
{
	public class StudentApiAddress
	{
		public readonly Uri Value;
		public StudentApiAddress(string value)		
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentNullException(nameof(value));
			Value = new Uri(value);
		}
	}
}
	