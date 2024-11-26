using Users.Application.UseCases.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Users.Domain.Interfaces
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
