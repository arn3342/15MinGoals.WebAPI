using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Users.DbAccess;

namespace Users.WebAPI
{
    public class Program
    {
        AccessUser accessUser;
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            //accessUser = new AccessUser("");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}