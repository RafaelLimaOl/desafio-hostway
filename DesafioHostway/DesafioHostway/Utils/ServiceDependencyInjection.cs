using DesafioHostway.Interfaces.Service;
using DesafioHostway.Services;

namespace DesafioHostway.Utils;

public static class ServiceDependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICarSpaceService, CarSpaceService>();
        services.AddScoped<IUserAccountService, UserAccountService>();
        services.AddScoped<IParkingLotService, ParkingLotService>();
        services.AddScoped<IParkingSessionService, ParkingSessionService>();

        return services;
    }
}
