using Smirnov_AP_kt_42_21.Interfaces.WorkloadInterfaces;
namespace Smirnov_AP_kt_42_21.ServiceExtensions
{
    public static class SevrviceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IWorkloadService, WorkloadService>();
            return services;
        }
    }
}