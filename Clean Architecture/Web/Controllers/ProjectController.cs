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
                return BadRequest(new { Message = "Invalid project data" });

            var createdProject = await _createProject.ExecuteAsync(projectDTO);
            return CreatedAtAction(null, createdProject);
        }

        [HttpGet("{projectId:guid}", Name = "GetProject")]
        public async Task<IActionResult> GetAsync(Guid projectId)
        {
            var project = await _readProject.ExecuteAsync(projectId);
            return project == null
                ? NotFound(new { Message = "Project not found" })
                : Ok(project);
        }

        [HttpGet(Name = "GetAllProject")]
        public async Task<IActionResult> GetAllAsync(int take, int skip)
        {
            var projects = await _readProjects.ExecuteAsync(new Pagination(take, skip));
            return Ok(projects);
        }

        [HttpDelete("{projectId:guid}", Name = "DeleteProject")]
        public async Task<IActionResult> DeleteAsync(Guid projectId)
        {
            var project = await _deleteProject.ExecuteAsync(projectId);
            return project == null
                ? NotFound(new { Message = "Project not found" })
                : Ok(project);
        }

        [HttpPatch("{projectId:guid}", Name = "UpdateProject")]
        public async Task<IActionResult> UpdateAsync([FromBody] ProjectUpdateDTO projectDTO, Guid projectId)
        {
            var project = await _updateProject.ExecuteAsync(projectDTO, projectId);
            return project == null
                ? NotFound(new { Message = "Project not found" })
                : Ok(project);
        }
    }
}
