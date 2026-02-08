using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Interfaces.Service;
using DesafioHostway.Models.Response;
using DesafioHostway.Models.Response.Wrappers;

namespace DesafioHostway.Services;

public class DashService(IDashRepository dashRepository) : IDashService
{
    private readonly IDashRepository _dashRepository = dashRepository;

    public async Task<ServiceResponse<IEnumerable<RevenueByDayResponse>>> GetRevenueByDay(int days)
    {
        if (days != 7 && days != 30)
            days = 7;

        var result = await _dashRepository.GetRevenueByDay(days);

        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<RevenueByDayResponse>>.Ok("Sem faturamento no período");

        return ServiceResponse<IEnumerable<RevenueByDayResponse>>.Ok(result);
    }

    public async Task<ServiceResponse<IEnumerable<TopSessionCarsResponse>>> GetTopCars(DateTime start, DateTime end)
    {
        if (end < start)
            return ServiceResponse<IEnumerable<TopSessionCarsResponse>>.Fail("Período inválido");

        if ((end - start).TotalDays > 365)
            return ServiceResponse<IEnumerable<TopSessionCarsResponse>>.Fail("Período muito grande");

        var result = await _dashRepository.GetTopCars(start, end);

        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<TopSessionCarsResponse>>.Ok("Sem dados no período");

        return ServiceResponse<IEnumerable<TopSessionCarsResponse>>.Ok(result);
    }

    public async Task<ServiceResponse<IEnumerable<SpaceOccupancyResponse>>> GetOccupancy(DateTime start, DateTime end)
    {
        if (end < start)
            return ServiceResponse<IEnumerable<SpaceOccupancyResponse>>.Fail("Período inválido");

        if ((end - start).TotalDays > 60)
            return ServiceResponse<IEnumerable<SpaceOccupancyResponse>>.Fail("Período máximo de 60 dias");

        var result = await _dashRepository.GetOccupancy(start, end);

        if (result == null || !result.Any())
            return ServiceResponse<IEnumerable<SpaceOccupancyResponse>>.Ok("Sem dados de ocupação");

        return ServiceResponse<IEnumerable<SpaceOccupancyResponse>>.Ok(result);
    }
}
