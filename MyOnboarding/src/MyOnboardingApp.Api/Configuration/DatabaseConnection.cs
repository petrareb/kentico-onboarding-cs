using System;
using System.Configuration;
using MyOnboardingApp.Contracts.Repository;

namespace MyOnboardingApp.Api.Configuration
{
    internal class DatabaseConnection : IDatabaseConnection
    {
        private static readonly Lazy<string> s_databaseConnectionString
            = new Lazy<string>(()
                => ConfigurationManager
                    .ConnectionStrings["TodoListDbConnection"]
                    .ConnectionString);

        public string DatabaseConnectionString
            => s_databaseConnectionString.Value;
    }
}
