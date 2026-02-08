using Dapper;
using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Models.Response;
using System.Data;

namespace DesafioHostway.Repositories;

public class DashRepository(IDbConnection dbConnection) : IDashRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;
    public async Task<IEnumerable<RevenueByDayResponse>> GetRevenueByDay(int days)
    {
        const string sql = @"
            SELECT
                DATE(ExitTime) AS Day,
                SUM(AmountChanged) AS TotalAmount
            FROM ParkingSession
            WHERE ExitTime IS NOT NULL
              AND ExitTime >= DATE('now', '-' || @days || ' day')
            GROUP BY DATE(ExitTime)
            ORDER BY Day;";

        return await _dbConnection.QueryAsync<RevenueByDayResponse>(sql, new { days });
    }

    public async Task<IEnumerable<TopSessionCarsResponse>> GetTopCars(DateTime start, DateTime end)
    {
        const string sql = @" SELECT
                c.LicensePlate,
                SUM(
                    (julianday(COALESCE(ps.ExitTime, CURRENT_TIMESTAMP)) -
                     julianday(ps.EntryTime)) * 24 * 60
                ) AS TotalMinutes
            FROM ParkingSession ps
            JOIN Car c ON c.Id = ps.CarId
            WHERE ps.EntryTime >= @startDate
              AND ps.EntryTime <= @endDate
            GROUP BY c.Id, c.LicensePlate
            ORDER BY TotalMinutes DESC
            LIMIT 10;";

        return await _dbConnection.QueryAsync<TopSessionCarsResponse>(sql, new { startDate = start, endDate = end });
    }

    public async Task<IEnumerable<SpaceOccupancyResponse>> GetOccupancy(DateTime start, DateTime end)
    {
        const string sql = @"
            WITH RECURSIVE hours(h) AS (
                SELECT datetime(@start)
                UNION ALL
                SELECT datetime(h, '+1 hour')
                FROM hours
                WHERE h < @end
            )
            SELECT
                h AS Hour,
                COUNT(ps.Id) AS Occupancy
            FROM hours
            LEFT JOIN ParkingSession ps
                ON ps.EntryTime <= h
               AND (ps.ExitTime IS NULL OR ps.ExitTime > h)
            GROUP BY h
            ORDER BY h;";

        return await _dbConnection.QueryAsync<SpaceOccupancyResponse>(sql, new { start, end });
    }

}
