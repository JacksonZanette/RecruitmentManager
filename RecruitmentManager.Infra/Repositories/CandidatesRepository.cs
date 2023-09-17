using Dapper;
using Microsoft.Data.Sqlite;
using RecruitmentManager.Domain.Entities;
using RecruitmentManager.Domain.Interfaces.Repositories;
using RecruitmentManager.Infra.Database;

namespace RecruitmentManager.Infra.Repositories
{
    public class CandidatesRepository : ICandidatesRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public CandidatesRepository(DatabaseConfig databaseConfig)
            => _databaseConfig = databaseConfig;

        public async Task CreateAsync(Candidate candidate)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.ExecuteAsync("INSERT INTO Candidate (Name, Score) VALUES (@Name, @Score);", candidate);
        }

        public async Task<IEnumerable<Candidate>> GetAsync()
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            return await connection.QueryAsync<Candidate>("SELECT Id, Name, Score FROM Candidate;");
        }

        public async Task<Candidate> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            return await connection.QueryFirstOrDefaultAsync<Candidate>("SELECT Id, Name, Score FROM Candidate WHERE Id = @id;", new { id });
        }

        public async Task UpdateAsync(Candidate candidate)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.ExecuteAsync("UPDATE Candidate SET Name = @Name, Score = @Score WHERE Id = @Id;", candidate);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.ExecuteAsync("DELETE FROM Candidate WHERE Id = @id;", new { id });
        }
    }
}