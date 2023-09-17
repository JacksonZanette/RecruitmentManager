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

            var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'Candidate';");
            var tableName = table.FirstOrDefault();
            if (!string.IsNullOrEmpty(tableName) && tableName == "Candidate")
                return;

            connection.Execute(@"CREATE TABLE Candidate (Id INTEGER PRIMARY KEY, Name TEXT NOT NULL, Score INTEGER NULL);");
        }
    }
}