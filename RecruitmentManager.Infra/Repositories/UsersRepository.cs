﻿using Dapper;
using Microsoft.Data.Sqlite;
using RecruitmentManager.Domain.Entities;
using RecruitmentManager.Domain.Interfaces.Repositories;
using RecruitmentManager.Infra.Database;

namespace RecruitmentManager.Infra.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public UsersRepository(DatabaseConfig databaseConfig)
            => _databaseConfig = databaseConfig;

        public async Task CreateAsync(User user)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.ExecuteAsync("INSERT INTO User (Name, Password) VALUES (@Name, @Password);", user);
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            return await connection.QueryAsync<User>("SELECT Id, Name, Password FROM User;");
        }

        public async Task<bool> ExistsByNameAndPassword(string name, string? password = default)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            var command = $"SELECT COUNT(1) FROM User WHERE Name = @name{(password is not null ? " AND Password = @password" : string.Empty)};";
            return await connection.ExecuteScalarAsync<bool>(command, new { name, password });
        }
    }
}