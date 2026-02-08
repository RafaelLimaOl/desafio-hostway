using Dapper;
using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using System.Data;

namespace DesafioHostway.Repositories;

public class CarSpaceRepository(IDbConnection dbConnection) : ICarSpaceRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<IEnumerable<CarSpaceResponse>> GetAllCarsSpaces(Guid userId)
    {
        const string query = @"SELECT Id, SpaceNumber, IsAccessibleParkingSpace, ParkingLotId, IsActive WHERE UserId = @UserId";
        return await _dbConnection.QueryAsync<CarSpaceResponse>(query, new { UserId = userId });
    }

    public async Task<IEnumerable<CarSpaceResponse>> GetAllSpacesByParkingLot(Guid parkingLotId)
    {
        const string query = @"SELECT Id, SpaceNumber, IsAccessibleParkingSpace, ParkingLotId, IsActive FROM CarSpaces WHERE  ParkingLotId = @ParkingLotId";
        return await _dbConnection.QueryAsync<CarSpaceResponse>(query, new { ParkingLotId = parkingLotId });
    }

    public async Task<CarSpaceResponse?> GetCarSpaceById(Guid spaceId)
    {
        const string query = @"SELECT Id, SpaceNumber, IsAccessibleParkingSpace, ParkingLotId, IsActive FROM CarSpaces WHERE Id = @SpaceId";
        return await _dbConnection.QueryFirstOrDefaultAsync<CarSpaceResponse>(query, new { SpaceId = spaceId });
    }

    public async Task<CarSpaceResponse> CreateCarSpace(CarSpaceRequest request)
    {
        var id = Guid.NewGuid().ToString().ToUpper();
        var createdAt = DateTime.Now;

        const string query = @"INSERT INTO CarSpaces (Id, SpaceNumber, IsAccessibleParkingSpace, ParkingLotId, IsActive, CreatedAt) VALUES(@Id, @SpaceNumber, @IsAccessibleParkingSpace, @ParkingLotId, @IsActive, @CreatedAt) RETURNING Id";
        var returnedId = await _dbConnection.ExecuteScalarAsync<Guid>(query, new
        {
            Id = id,
            request.SpaceNumber,
            request.IsAccessibleParkingSpace,
            request.ParkingLotId,
            request.IsActive,
            CreatedAt = createdAt
        });

        return new CarSpaceResponse
        {
            Id = returnedId,
            SpaceNumber = request.SpaceNumber,
            IsAccessibleParkingSpace = request.IsAccessibleParkingSpace,
            ParkingLotId = request.ParkingLotId,
            IsActive = request.IsActive
        };
    }

    public async Task<bool> UpdateCarSpace(Guid spaceId, CarSpaceRequest request)
    {
        const string query = @"UPDATE CarSpaces SET SpaceNumber = @SpaceNumber, IsAccessibleParkingSpace = @IsAccessibleParkingSpace, IsActive = @IsActive, ParkingLotId = @ParkingLotId, UpdatedAt = @UpdatedAt WHERE Id = @SpaceId";
        var rows = await _dbConnection.ExecuteAsync(query, new
        {
            SpaceId = spaceId,
            request.SpaceNumber,
            request.IsAccessibleParkingSpace,
            request.ParkingLotId,
            request.IsActive,

            UpdatedAt = DateTime.Now,
        });

        return rows > 0;
    }

    public async Task<bool> DeleteCarSpace(Guid spaceId)
    {
        const string query = @"DELETE FROM CarSpaces WHERE Id = @SpaceId";
        var rows = await _dbConnection.ExecuteAsync(query, new { SpaceId = spaceId });

        return rows > 0;
    }

    // Validation Queries:

    public async Task<bool> ExistSpaceById(Guid spaceId)
    {
        const string query = @"SELECT 1 FROM CarSpaces WHERE Id = @SpaceId";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { SpaceId = spaceId });
    }

    public async Task<bool> CarSpaceContainsInParkingLot(Guid spaceId, Guid parkingId)
    {
        const string query = @"SELECT 1 FROM CarSpaces WHERE Id = @SpaceId AND ParkingLotId = @ParkingId";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { SpaceId = spaceId, ParkingId = parkingId });
    }

    public async Task<bool> CurrentSpaceIsAvaliable(Guid spaceId)
    {
        const string query = @"";
        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { SpaceId = spaceId });
    }

    public async Task<bool> UserCanUseAccessibleParkingSpace(Guid spaceId, Guid userId)
    {
        const string query = @"";
        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { SpaceId = spaceId, UserId = userId });
    }

}
