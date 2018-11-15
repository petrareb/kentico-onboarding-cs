using System.Configuration;
using MyOnboardingApp.Contracts.Database;

namespace MyOnboardingApp.Api.Configuration
{
    internal class DatabaseConnection : IDatabaseConnection
    {
        private static readonly string s_databaseConnectionString =
            ConfigurationManager
                .ConnectionStrings["TodoListDbConnection"]
                .ConnectionString;


        public string GetDatabaseConnectionString()
            => s_databaseConnectionString;
    }
}
