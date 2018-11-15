namespace MyOnboardingApp.Contracts.Generators
{
    public interface IIdGenerator<out TId>
    {
        TId GetNewId();
    }
}