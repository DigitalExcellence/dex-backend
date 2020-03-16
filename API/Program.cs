using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace API
{
    /// <summary>
	/// the main starting point of the program.
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Main
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}
		/// <summary>
		/// The kestrel builder.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public static IWebHostBuilder CreateHostBuilder(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseKestrel(o => o.AddServerHeader = false);
		}
	}
}
