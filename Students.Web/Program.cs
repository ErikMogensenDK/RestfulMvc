using Students.Web.Clients;
using Students.Web.Clients.Values;

namespace Students.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			var apiAddressStr = builder.Configuration.GetValue<string>("StudentApiAddress");
			var apiAddress = new StudentApiAddress(apiAddressStr!);
			builder.Services.AddSingleton(apiAddress);
			builder.Services.AddHttpClient<IStudentApiClient, StudentApiClient>();

			var app = builder.Build();

			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthorization();

			app.MapStaticAssets();
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}")
				.WithStaticAssets();

			app.Run();
		}
	}
}
