using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clean_Architecture.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetById(Guid id);
        Task<IEnumerable<T>> GetAll(Pagination pagination);
        Task Add(T entity);
        Task<T?> Update(T entity, Guid id);
        Task<T?> Delete(Guid id);
    }
}
