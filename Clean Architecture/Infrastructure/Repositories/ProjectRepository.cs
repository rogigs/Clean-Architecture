using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;
using Clean_Architecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clean_Architecture.Infrastructure.Repositories
{
    public class ProjectRepository(AppDbContext context) : IProjectRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Project?> GetById(Guid projectId)
        {
            return await _context.Projects.FindAsync(projectId);
        }

        public async Task<IEnumerable<Project>> GetAll(
            PaginationDTO pagination)
        {
            return await _context.Projects.OrderBy(val => val.StartDate).Skip(pagination.Skip).Take(pagination.Take).ToListAsync();
        }

        public async Task Add(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task<Project?> Update(ProjectUpdateDTO projectDTO, Guid projectId)
        {
            var projectDB = await _context.Projects.FindAsync(projectId);

            if (projectDB == null) { return null; }

            if (!string.IsNullOrEmpty(projectDTO?.Name))
            {
                projectDB.Name = projectDTO.Name;
            }

            if (!string.IsNullOrEmpty(projectDTO?.Description))
            {
                projectDB.Description = projectDTO.Description;
            }

            if (projectDTO?.EndDate.HasValue == true)
            {
                projectDB.EndDate = projectDTO.EndDate.Value;
            }

            _context.Projects.Update(projectDB);

            await _context.SaveChangesAsync();

            return projectDB;
        }

        public async Task<Project?> Delete(Guid projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);

            if (project == null)
            {
                return null;
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return project;

        }
    }
}
