namespace Clean_Architecture.Domain.Interfaces
{
    public interface IUseCase<TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest request);
        Task<TResponse> ExecuteAsync(TRequest request, Guid id) => throw new NotImplementedException();

    }
}
