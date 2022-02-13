using ClosedXML.Excel;
using FileSharing.Areas.Admin.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FileSharing.Areas.Admin
{
    public static class AdminStartUp
    {
        public static IServiceCollection addAdminServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserServices>();
            services.AddTransient<IXLWorkbook, XLWorkbook>();
            services.AddTransient<IContactUsService, ContactUsService>();
            return services;
        }
    }
}
