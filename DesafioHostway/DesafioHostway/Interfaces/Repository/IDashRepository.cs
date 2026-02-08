using DesafioHostway.Models.Response;

namespace DesafioHostway.Interfaces.Repository;

public interface IDashRepository
{
    Task<IEnumerable<RevenueByDayResponse>> GetRevenueByDay(int days);
    Task<IEnumerable<TopSessionCarsResponse>> GetTopCars(DateTime start, DateTime end);
    Task<IEnumerable<SpaceOccupancyResponse>> GetOccupancy(DateTime start, DateTime end);

}
