using Dapper;
using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using System.Data;

namespace DesafioHostway.Repositories;

public class CarRepository(IDbConnection dbConnection) : ICarRepository 
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<IEnumerable<CarResponse>> GetAllCars(Guid userId)
    {
        const string query = @"SELECT Id, LicensePlate, Color, Model, IsActive, UserId FROM Cars WHERE UserId = @UserId";
        return await _dbConnection.QueryAsync<CarResponse>(query, new { UserId = userId });
    }

    public async Task<CarResponse?> GetCarById(Guid carId)
    {
        const string query = @"SELECT Id, LicensePlate, Color, Model, IsActive, UserId FROM Cars WHERE Id = @CarId";
        return await _dbConnection.QueryFirstOrDefaultAsync<CarResponse>(query, new { CarId = carId });
    }

    public async Task<CarResponse> CreateCar(CarRequest request)
    {
        var id = Guid.NewGuid().ToString().ToUpper();
        var createdAt = DateTime.UtcNow;

        const string query = @"INSERT INTO Cars(Id, LicensePlate, Color, Model, UserId, IsActive, CreatedAt) VALUES(@Id, @LicensePlate, @Color, @Model, @UserId, @IsActive, @CreatedAt) RETURNING Id";
        var returnedId = await _dbConnection.ExecuteScalarAsync<Guid>(query, new
        {
            Id = id,
            request.LicensePlate,
            request.Color,
            request.Model,
            request.UserId,
            IsActive = true,
            CreatedAt = createdAt
        });

        return new CarResponse
        {
            Id = returnedId,
            LicensePlate = request.LicensePlate,
            Model = request.Model,
            Color = request.Color,
            UserId = request.UserId,
        };
    }

    public async Task<bool> UpdateCar(Guid carId, CarRequest request)
    {
        const string query = @"UPDATE Cars SET LicensePlate = @LicensePlate, Color = @Color, Model = @Model, IsActive = @IsActive, UpdatedAt = @UpdatedAt WHERE Id = @CarId";
        var rows = await _dbConnection.ExecuteAsync(query, new
        {
            request.LicensePlate,
            request.Color,
            request.Model,
            request.IsActive,
            UpdatedAt = DateTime.Now,

            CarId = carId
        });

        return rows > 0;
    }

    public async Task<bool> DeleteCar(Guid carId)
    {
        const string query = @"DELETE FROM Cars WHERE Id = @CarId";
        var rows = await _dbConnection.ExecuteAsync(query, new { CarId = carId });

        return rows > 0;
    }

    // Validation Queries:

    public async Task<bool> ExistCarById(Guid carId)
    {
        const string query = @"SELECT COUNT(1) FROM Cars WHERE Id = @CarId";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { CarId = carId });
    }

    public async Task<bool> CarContainsInUser(Guid carId, Guid userId)
    {
        const string query = @"SELECT 1 FROM Cars WHERE Id = @CarId AND UserId = @UserId";

        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { CarId = carId, UserId = userId });
    }

    public async Task<bool> CarExistsByLicensePlate(Guid? carId, string licensePlate)
    {
        const string query = @"SELECT 1 FROM Cars WHERE LicensePlate = @LicensePlate AND (@CarId IS NULL OR Id <> @CarId)";

        var result = await _dbConnection.ExecuteScalarAsync<int?>(
            query,
            new { LicensePlate = licensePlate, CarId = carId }
        );

        return result.HasValue;
    }
}
