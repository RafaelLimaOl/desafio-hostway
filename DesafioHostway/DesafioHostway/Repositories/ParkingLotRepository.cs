using Dapper;
using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using System.Data;

namespace DesafioHostway.Repositories;

public class ParkingLotRepository(IDbConnection dbConnection) : IParkingLotRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<IEnumerable<ParkingLotResponse>> GetAllParkingLots(Guid userId)
    {
        const string query = @"
            SELECT 
                p.Id,
                p.Name,
                p.Address,
                p.FirstHourAmount,
                p.ExtraHourAmount,
                p.UserId,
                p.IsActive,
                COUNT(cs.Id) AS LotsAvaliable
            FROM ParkingLots p
            LEFT JOIN CarSpaces cs 
                ON cs.ParkingLotId = p.Id
            WHERE p.UserId = @UserId
            GROUP BY 
                p.Id, p.Name, p.Address, p.FirstHourAmount, p.ExtraHourAmount, p.UserId, p.IsActive";

        return await _dbConnection.QueryAsync<ParkingLotResponse>(query, new { UserId = userId });
    }

    public async Task<ParkingLotResponse?> GetParkingLotById(Guid parkingId)
    {
        const string query = @"SELECT Id, Name, Address, FirstHourAmount, ExtraHourAmount, UserId, IsActive FROM ParkingLots WHERE Id = @ParkingId";
        return await _dbConnection.QueryFirstOrDefaultAsync<ParkingLotResponse>(query, new { ParkingId = parkingId });
    }

    public async Task<IEnumerable<ParkingLotResponse>> GetAllParkingLotAvaliable()
    {
        const string query = @"SELECT Id, Name, Address, FirstHourAmount, ExtraHourAmount, UserId, IsActive FROM ParkingLots WHERE IsActive = 1";
        return await _dbConnection.QueryAsync<ParkingLotResponse>(query);
    }
    public async Task<ParkingLotWithCarSpacesResponse> GetParkingLotWithCarSpaces(Guid parkingLotId)
    {
        const string query = @"
            SELECT
                pl.Id,
                pl.Name,
                pl.Address,
                pl.FirstHourAmount,
                pl.ExtraHourAmount,
                pl.UserId,
                pl.IsActive,

                cs.Id AS CarSpaceId,
                cs.SpaceNumber,
                cs.ParkingLotId,
                cs.IsAccessibleParkingSpace,
                cs.IsActive AS CarSpaceIsActive
            FROM ParkingLots pl
            LEFT JOIN CarSpaces cs 
                ON cs.ParkingLotId = pl.Id
            WHERE pl.Id = @ParkingId;";

        var parkingLotDict = new Dictionary<Guid, ParkingLotWithCarSpacesResponse>();

        var result = await _dbConnection.QueryAsync<
            ParkingLotWithCarSpacesResponse, CarSpaceWithParkingLot, ParkingLotWithCarSpacesResponse>(
            query,
            (parkingLot, carSpace) =>
            {
                if (!parkingLotDict.TryGetValue(parkingLot.Id, out var parkingLotEntry))
                {
                    parkingLotEntry = parkingLot;
                    parkingLotEntry.CarSpaces = [];
                    parkingLotDict.Add(parkingLotEntry.Id, parkingLotEntry);
                }

                if (carSpace != null && carSpace.CarSpaceId != Guid.Empty)
                {
                    parkingLotEntry.CarSpaces.Add(carSpace);
                }

                return parkingLotEntry;
            },
            new { ParkingId = parkingLotId },
            splitOn: "CarSpaceId"
        );

        return parkingLotDict.Values.FirstOrDefault();

    }
    public async Task<ParkingLotResponse> CreateNewParkingLot(ParkingLotRequest request)
    {
        var id = Guid.NewGuid().ToString().ToUpper();
        var createdAt = DateTime.UtcNow;

        const string query = @"INSERT INTO ParkingLots(Id, Name, Address, FirstHourAmount, ExtraHourAmount, UserId, IsActive, CreatedAt) VALUES(@Id, @Name, @Address, @FirstHourAmount, @ExtraHourAmount, @UserId, @IsActive, @CreatedAt) RETURNING Id ";
        var returnedId = await _dbConnection.ExecuteScalarAsync<Guid>(query, new
        {
            Id = id,
            request.Name,
            request.Address,
            request.FirstHourAmount,
            request.ExtraHourAmount,
            request.UserId,
            request.IsActive,
            CreatedAt = createdAt
        });

        return new ParkingLotResponse
        {
            Id = returnedId,
            Name = request.Name,
            Address = request.Address,
            FirstHourAmount = request.FirstHourAmount,
            ExtraHourAmount = request.ExtraHourAmount,
            IsActive = request.IsActive,
            UserId = request.UserId,
        };
    }

    public async Task<bool> UpdateParkingLot(Guid parkingId, ParkingLotRequest request)
    {
        const string query = @"UPDATE ParkingLots SET Name = @Name, Address = @Address, FirstHourAmount = @FirstHourAmount, ExtraHourAmount = @ExtraHourAmount, IsActive = @IsActive, UpdatedAt = @UpdatedAt WHERE Id = @ParkingId";
        var rows = await _dbConnection.ExecuteAsync(query, new
        {
            request.Name,
            request.Address,
            request.FirstHourAmount,
            request.ExtraHourAmount,
            request.IsActive,
            UpdatedAt = DateTime.Now,

            ParkingId = parkingId
        });

        return rows > 0;
    }

    public async Task<bool> DeleteParking(Guid parkingId)
    {
        const string query = @"DELETE FROM ParkingLots WHERE Id = @ParkingId";
        var rows = await _dbConnection.ExecuteAsync(query, new { ParkingId = parkingId });

        return rows > 0;
    }

    public async Task<int> CountSpacesInParkingById(Guid parkingId, bool activeSpaces)
    {
        const string query = @"SELECT COUNT(*) FROM CarSpaces WHERE ParkingLotId = @ParkingId AND IsActive = @SearchInactive";

        return await _dbConnection.ExecuteScalarAsync<int>(query, new
        {
            ParkingId = parkingId,
            SearchInactive = activeSpaces
        });
    }

    // Validation Queries:

    public async Task<bool> ExistParkingById(Guid parkingId)
    {
        const string query = @"SELECT COUNT(1) FROM ParkingLots WHERE Id = @ParkingId";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { ParkingId = parkingId });
    }

    public async Task<bool> ParkingContainsInUser(Guid parkingId, Guid userId)
    {
        const string query = @"SELECT 1 FROM ParkingLots WHERE Id = @ParkingId AND UserId = @UserId";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { ParkingId = parkingId, UserId = userId });
    }
}
