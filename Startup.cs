using FileSharing.Areas.Admin;
using FileSharing.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileSharing
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddViewLocalization(options =>
                {
                    options.ResourcesPath = "Resources";
                });
            //DbContext Context service
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // Idetity service
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();


            //localization service
            services.AddLocalization();

            // Repository pattern services
            services.addAdminServices();

            // Auto Mapper service
            services.AddAutoMapper(typeof(Startup));

            //Login with Google or facebook
            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = Configuration["Authentication:Facebook:AppId"];
                    options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                });
            //.AddGoogle(options =>
            //{
            //    options.ClientId = Configuration["Authentication:Google:ClientId"];
            //    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //Localization
            var supportedCulture = new[] { "ar-SA", "en-US" };
            app.UseRequestLocalization(e =>
            {
                e.AddSupportedUICultures(supportedCulture);
                e.AddSupportedCultures(supportedCulture);
                e.SetDefaultCulture("en-US");
            });

            app.UseRouting();

            //Jwt (json web token)
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
             );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
