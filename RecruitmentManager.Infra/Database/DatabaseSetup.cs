using Dapper;
using Microsoft.Data.Sqlite;

namespace RecruitmentManager.Infra.Database
{
    public class DatabaseSetup
    {
        private readonly DatabaseConfig _databaseConfig;

        public DatabaseSetup(DatabaseConfig databaseConfig)
            => _databaseConfig = databaseConfig;

        public void Setup()
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            if (!connection.ExecuteScalar<bool>("SELECT COUNT(1) FROM sqlite_master WHERE type='table' AND name = 'Candidate';"))
                connection.Execute(@"CREATE TABLE Candidate (Id INTEGER PRIMARY KEY, Name TEXT NOT NULL, Score INTEGER NULL);");

            if (!connection.ExecuteScalar<bool>("SELECT COUNT(1) FROM sqlite_master WHERE type='table' AND name = 'User';"))
                connection.Execute(@"CREATE TABLE User (Id INTEGER PRIMARY KEY, Name TEXT NOT NULL, Password TEXT NOT NULL);");
        }
    }
}