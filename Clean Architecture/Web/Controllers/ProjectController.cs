using Clean_Architecture.Application.UseCases;
using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clean_Architecture.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase, IProjectController
    {
        private readonly ICreateProject _createProject;
        private readonly IReadProject _readProject;
        private readonly IReadProjects _readProjects;
        private readonly IDeleteProject _deleteProject;
        private readonly IUpdateProject _updateProject;

        public ProjectController(
            ICreateProject createProject,
            IReadProject readProject,
            IReadProjects readProjects,
            IDeleteProject deleteProject,
            IUpdateProject updateProject)
        {
            _createProject = createProject;
            _readProject = readProject;
            _readProjects = readProjects;
            _deleteProject = deleteProject;
            _updateProject = updateProject;
        }

        [HttpPost(Name = "PostProject")]
        public async Task<IActionResult> PostAsync([FromBody] ProjectDTO projectDTO)
        {
            if (projectDTO == null || string.IsNullOrWhiteSpace(projectDTO.Name))
                return BadRequest(new { Message = "Invalid Project data" });

            var (error, createdProject) = await _createProject.ExecuteAsync(projectDTO);

            return error == null ? CreatedAtAction(null, createdProject) : BadRequest(new { error.Message });
        }

        [HttpGet("{projectId:guid}", Name = "GetProject")]
        public async Task<IActionResult> GetAsync(Guid projectId)
        {
            var (error, project) = await _readProject.ExecuteAsync(projectId);

            if (error != null)
            {
                return BadRequest(new { error.Message });
            }

            return project == null
                ? NotFound(new { Message = "Project not found" })
                : Ok(project);
        }

        [HttpGet(Name = "GetAllProject")]
        public async Task<IActionResult> GetAllAsync(int take, int skip)
        {
            var (error, projects) = await _readProjects.ExecuteAsync(new Pagination(take, skip));
            return error == null ? Ok(projects) : BadRequest(new { error.Message });
        }

        [HttpDelete("{projectId:guid}", Name = "DeleteProject")]
        public async Task<IActionResult> DeleteAsync(Guid projectId)
        {
            var (error, project) = await _deleteProject.ExecuteAsync(projectId);

            if (error != null)
            {
                return BadRequest(new { error.Message });
            }

            return project == null
                ? NotFound(new { Message = "Project not found" })
                : Ok(project);
        }

        [HttpPatch("{projectId:guid}", Name = "UpdateProject")]
        public async Task<IActionResult> UpdateAsync([FromBody] ProjectUpdateDTO projectDTO, Guid projectId)
        {
            var (error, project) = await _updateProject.ExecuteAsync(projectDTO, projectId);

            if (error != null)
            {
                return BadRequest(new { error.Message });
            }

            return project == null
                ? NotFound(new { Message = "Project not found" })
                : Ok(project);
        }
    }
}
