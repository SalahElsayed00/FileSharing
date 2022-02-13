using FileSharing.Areas.Admin.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace FileSharing
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // run migrate on external server
            using (var scope = host.Services.CreateScope())
            {
                // -----------run migrate on external server-------------------//
                var provider = scope.ServiceProvider;
                //var dbContext = provider.GetRequiredService<ApplicationDbContext>();
                //dbContext.Database.Migrate();

                // ------------- call user service ----- method roles-----
                var userservices = provider.GetRequiredService<IUserService>();
                await userservices.InitializeAsync();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
