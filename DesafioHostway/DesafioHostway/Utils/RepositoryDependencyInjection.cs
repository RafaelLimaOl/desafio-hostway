using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Repositories;

namespace DesafioHostway.Utils;

public static class RepositoryDependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IUserAccountRepository, UserAccountRepository>();
        services.AddScoped<ICarSpaceRepository, CarSpaceRepository>();
        services.AddScoped<IParkingLotRepository, ParkingLotRepository>();
        services.AddScoped<IParkingLotRepository, ParkingLotRepository>();
        services.AddScoped<IParkingSessionRepository, ParkingSessionRepository>();

        return services;
    }
}
