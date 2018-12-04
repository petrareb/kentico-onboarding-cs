namespace MyOnboardingApp.Contracts.Validation
{
    public interface IResolvedItem<out TItem>
        where TItem : class
    {
        bool WasOperationSuccessful { get; }
        TItem Item { get; }
    }
}