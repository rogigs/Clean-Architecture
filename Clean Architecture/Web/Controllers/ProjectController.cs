using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clean_Architecture.Web.Controllers

{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ICreateProject _createProject;


        public ProjectController(ICreateProject createProject)
        {
            _createProject = createProject;
        }


        [HttpPost(Name = "PostProject")]
        public async Task<IActionResult> PostAsync([FromBody] ProjectDTO projectDTO)
        {
            if (projectDTO == null || string.IsNullOrWhiteSpace(projectDTO.Name))
            {
                return BadRequest(new { Message = "Invalid project data" });
            }


            var createdProject = await this._createProject.ExecuteAsync(projectDTO);


            return CreatedAtAction(null, createdProject);

        }
    }
}
