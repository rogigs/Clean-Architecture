using Microsoft.AspNetCore.Mvc;

namespace Clean_Architecture.Domain.Interfaces
{
    public interface IController<TDTO>
    {
        Task<IActionResult> PostAsync(TDTO dto);
        Task<IActionResult> GetAsync(Guid id);
    }
}
