using Users.Application.UseCases.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Users.Domain.Interfaces
{
    public interface IController<TDTO, TUpdateDTO>
    {
        Task<IActionResult> PostAsync(TDTO dto);
        Task<IActionResult> GetAsync(Guid id);
        Task<IActionResult> GetAllAsync(PaginationDTO paginationDTO);
        Task<IActionResult> DeleteAsync(Guid id);
        Task<IActionResult> UpdateAsync(TUpdateDTO tUpdateDTO, Guid id);
    }
}
