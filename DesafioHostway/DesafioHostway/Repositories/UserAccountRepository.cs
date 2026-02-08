using Dapper;
using DesafioHostway.Interfaces.Repository;
using DesafioHostway.Models.Request;
using DesafioHostway.Models.Response;
using System.Data;

namespace DesafioHostway.Repositories;

public class UserAccountRepository(IDbConnection dbConnection) : IUserAccountRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<bool> UserExistById(Guid userId)
    {
        const string query = @"SELECT 1 FROM Users WHERE Id = @UserId";
        return await _dbConnection.ExecuteScalarAsync<bool>(query, new { UserId = userId });
    }

    public async Task<UserAccountResponse?> GetUserInfo(Guid userId)
    {
        const string query = @"SELECT Id, Username, Email, Role, HaveDeficiency, IsActive FROM Users WHERE Id = @UserId";
        return await _dbConnection.QueryFirstOrDefaultAsync<UserAccountResponse?>(query, new { UserId = userId });
    }
    public async Task<bool> UpdateUserInfo(Guid userId, UserAccountRequest request)
    {
        const string query = @"UPDATE Users SET Username = @Username, Email = @Email, HaveDeficiency = @HaveDeficiency, IsActive = @IsActive, UpdatedAt = @UpdatedAt WHERE Id = @UserId";
        var rows = await _dbConnection.ExecuteAsync(query, new
        {
            UserId = userId,
            request.Username,
            request.Email,
            request.HaveDeficiency,
            request.IsActive,
            UpdatedAt = DateTime.Now,
        });

        return rows > 0;
    }

    public async Task<bool> DeleteUserAccount(Guid userId)
    {
        const string query = @"DELETE FROM Users WHERE Id = @UserId";
        var rows = await _dbConnection.ExecuteAsync(query, new { UserId = userId });

        return rows > 0;
    }

}
