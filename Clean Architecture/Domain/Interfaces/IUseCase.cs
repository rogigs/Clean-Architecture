namespace Clean_Architecture.Domain.Interfaces
{
    public interface IUseCase<TRequest, TResponse, TException> where TException : Exception
    {
        Task<(TException?, TResponse?)> ExecuteAsync(TRequest request);
        Task<(TException?, TResponse?)> ExecuteAsync(TRequest request, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
