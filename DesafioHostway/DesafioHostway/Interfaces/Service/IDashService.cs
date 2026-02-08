using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;

namespace DesafioHostway.Interfaces.Service;

public interface IDashService
{
    Task<ServiceResponse<IEnumerable<RevenueByDayResponse>>> GetRevenueByDay(int days);
    Task<ServiceResponse<IEnumerable<TopSessionCarsResponse>>> GetTopCars(DateTime start, DateTime end);
    Task<ServiceResponse<IEnumerable<SpaceOccupancyResponse>>> GetOccupancy(DateTime start, DateTime end);
}
