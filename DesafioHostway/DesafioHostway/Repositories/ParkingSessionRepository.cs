using Dapper;
using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using System.Data;

namespace DesafioHostway.Repositories;

public class ParkingSessionRepository(IDbConnection dbConnection) : IParkingSessionRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<ParkingSessionResponse> GetAllSessionByCar(Guid carId)
    {
        const string query = @"SELECT Id, CarId, SpaceId, EntryTime, ExitTime, AmountChanged, IsActive FROM ParkingSessions WHERE CarId = @CarId";

        return await _dbConnection.QueryFirstOrDefaultAsync<ParkingSessionResponse>(query, new { CarId = carId });
    }

    public async Task<IEnumerable<ParkingSessionResponse>> GetAllSessionsByUser(Guid userId)
    {
        const string query = @"
        SELECT 
            ps.Id,
            ps.CarId,
            ps.SpaceId,
            ps.EntryTime,
            ps.ExitTime,
            ps.TotalTime,
            ps.AmountChanged,
            ps.IsActive
        FROM ParkingSessions ps
        INNER JOIN Cars c ON c.Id = ps.CarId
        WHERE c.UserId = @UserId";

        return await _dbConnection.QueryAsync<ParkingSessionResponse>(
            query,
            new { UserId = userId }
        );
    }

    public async Task<ParkingSessionResponse> GetAllSessionBySpaceId(Guid spaceId)
    {
        const string query = @"SELECT Id, CarId, SpaceId, EntryTime, ExitTime, AmountChanged, IsActive FROM ParkingSessions WHERE SpaceID = @SpaceId";

        return await _dbConnection.QueryFirstOrDefaultAsync<ParkingSessionResponse>(query, new { SpaceId = spaceId });
    }

    public async Task<ParkingSessionResponse?> GetSessionById(Guid id)
    {
        const string query = @"SELECT Id, CarId, SpaceId, EntryTime, ExitTime, AmountChanged, IsActive FROM ParkingSessions WHERE Id = @Id";

        return await _dbConnection.QueryFirstOrDefaultAsync<ParkingSessionResponse>(query, new { Id = id });
    }

    public async Task<ParkingSessionResponse> CreateNewSession(ParkinSessionCreateRequest request)
    {
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        const string query = @"INSERT INTO ParkingSessions (Id, CarId, SpaceId, EntryTime, IsActive, CreatedAt) VALUES (@Id, @CarId, @SpaceId, @EntryTime, @IsActive, @CreatedAt) RETURNING Id";

        var returnedId = await _dbConnection.ExecuteScalarAsync<Guid>(query, new
        {
            Id = id,
            request.CarId,
            request.SpaceId,
            EntryTime = DateTime.UtcNow,
            IsActive = true,
            CreatedAt = createdAt
        });

        return new ParkingSessionResponse
        {
            Id = returnedId,
            CarId = request.CarId,
            SpaceId = request.SpaceId,
            EntryTime = DateTime.UtcNow,
            IsActive = true
        };
    }

    public async Task<bool> CloseCurrentSession(Guid sessionId, DateTime exitTime, decimal amount, TimeSpan totalTime)
    {
        const string query = @"UPDATE ParkingSessions SET ExitTime = @ExitTime, AmountChanged = @Amount, TotalTime = @TotalTime, IsActive = false, UpdatedAt = @UpdatedAt WHERE Id = @SessionId";

        var rows = await _dbConnection.ExecuteAsync(query, new
        {
            ExitTime = exitTime,
            Amount = amount,
            TotalTime = totalTime,
            UpdatedAt = DateTime.UtcNow,
            SessionId = sessionId
        });

        return rows > 0;
    }

    public async Task<bool> DeleteSession(Guid id)
    {
        const string query = @"DELETE FROM ParkingSessions WHERE Id = @Id";
        var rows = await _dbConnection.ExecuteAsync(query, new { Id = id });

        return rows > 0;
    }

    // Validation Queries
    public async Task<ParkingPriceResponse?> GetParkingPrice(Guid sessionId)
    {
        const string query = @"
            SELECT 
                pl.FirstHourAmount,
                pl.ExtraHourAmount
            FROM ParkingSessions ps
            INNER JOIN CarSpaces cs ON cs.Id = ps.SpaceId
            INNER JOIN ParkingLots pl ON pl.Id = cs.ParkingLotId
            WHERE ps.Id = @SessionId";

        return await _dbConnection.QueryFirstOrDefaultAsync<ParkingPriceResponse?>(query, new { SessionId = sessionId });
    }

    public async Task<bool> ExistSessionById(Guid id)
    {
        const string query = @"SELECT COUNT(1) FROM ParkingSessions WHERE Id = @Id";
        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { Id = id });
    }

    public async Task<bool> HasOpenSession(Guid carId)
    {
        const string query = @"SELECT 1 FROM ParkingSessions WHERE CarId = @CarId AND ExitTime IS NULL";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { CarId = carId });
    }
}
