namespace MyOnboardingApp.Contracts.Repository
{
    public interface IDatabaseConnection
    {
        string DatabaseConnectionString { get; }
    }
}