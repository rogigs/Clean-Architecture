using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Application.Validations;
using Clean_Architecture.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clean_Architecture.Web.Controllers
{
    [ApiController]
    public class ProjectController(
        ICreateProject createProject,
        IReadProject readProject,
        IReadProjects readProjects,
        IDeleteProject deleteProject,
        IUpdateProject updateProject) : ControllerBase, IProjectController
    {
        private readonly ICreateProject _createProject = createProject;
        private readonly IReadProject _readProject = readProject;
        private readonly IReadProjects _readProjects = readProjects;
        private readonly IDeleteProject _deleteProject = deleteProject;
        private readonly IUpdateProject _updateProject = updateProject;

        [HttpPost(Name = "PostProject")]
        public async Task<IActionResult> PostAsync([FromBody] ProjectDTO projectDTO)
        {
            var (error, createdProject) = await _createProject.ExecuteAsync(projectDTO);

            return error == null ? CreatedAtAction(null, createdProject) : BadRequest(new { error.Message });
        }

        [HttpGet("{projectId:guid}", Name = "GetProject")]
        public async Task<IActionResult> GetAsync(Guid projectId)
        {
            var (error, project) = await _readProject.ExecuteAsync(projectId);

            if (error != null) return BadRequest(new { error.Message });

            return project == null
                ? NotFound(new { Message = "Project not found" })
                : Ok(project);
        }

        [HttpGet(Name = "GetAllProject")]
        [ValidatePaginationAttributes]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDTO paginationDTO)
        {
            var (error, projects) = await _readProjects.ExecuteAsync(paginationDTO);

            return error == null ? Ok(projects) : BadRequest(new { error.Message });
        }

        [HttpDelete("{projectId:guid}", Name = "DeleteProject")]
        public async Task<IActionResult> DeleteAsync(Guid projectId)
        {
            var (error, project) = await _deleteProject.ExecuteAsync(projectId);

            if (error != null) return BadRequest(new { error.Message });

            return project == null
                ? NotFound(new { Message = "Project not found" })
                : Ok(project);
        }

        [HttpPatch("{projectId:guid}", Name = "UpdateProject")]
        [ValidateUpdateProjectAttributes]
        public async Task<IActionResult> UpdateAsync([FromBody] ProjectUpdateDTO projectDTO, Guid projectId)
        {
            var (error, project) = await _updateProject.ExecuteAsync(projectDTO, projectId);

            if (error != null) return BadRequest(new { error.Message });

            return project == null
                ? NotFound(new { Message = "Project not found" })
                : Ok(project);
        }
    }
}
