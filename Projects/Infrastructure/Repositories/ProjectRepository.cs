using Projects.Application.UseCases.DTO;
using Projects.Domain.Entities;
using Projects.Domain.Interfaces;
using Projects.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Projects.Infrastructure.Repositories
{
    public class ProjectRepository(AppDbContext context) : IProjectRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Project?> GetById(Guid projectId) => await _context.Projects.FindAsync(projectId);

        public async Task<IEnumerable<Project>> GetAll(PaginationDTO pagination) => await _context.Projects.OrderBy(val => val.StartDate).Skip(pagination.Skip).Take(pagination.Take).ToListAsync();

        public async Task Add(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task<Project?> Update(ProjectUpdateDTO projectUpdateDTO, Guid projectId)
        {
            var projectDB = await _context.Projects.FindAsync(projectId);

            if (projectDB == null) return null;

            projectDB.Name = string.IsNullOrEmpty(projectUpdateDTO?.Name) ? projectDB.Name : projectUpdateDTO.Name;
            projectDB.Description = string.IsNullOrEmpty(projectUpdateDTO?.Description) ? projectDB.Description : projectUpdateDTO.Description;
            projectDB.EndDate = projectUpdateDTO?.EndDate.HasValue == true ? projectUpdateDTO.EndDate.Value : projectDB.EndDate;
            projectDB.UsersId = [.. projectDB.UsersId, ..projectUpdateDTO?.UsersId];

            _context.Projects.Update(projectDB);

            await _context.SaveChangesAsync();

            return projectDB;
        }

        public async Task<Project?> Delete(Guid projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);

            if (project == null) return null;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return project;

        }
    }
}
