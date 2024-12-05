namespace Users.Domain.Interfaces
{
    public interface IAsyncInitialization<E, R> where E : Exception
    {
        Task<(E?, R)> InitializeAsync();
    }

    public interface IAsyncInitializationRetry<E, R> where E : Exception
    {
        Task InitializeAsync();
        Task<(E?, R)> InitializeAsyncRetryAsync(int maxRetries = 0);
    }
}
