﻿using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;


namespace Clean_Architecture.Application.UseCases
{
    public class CreateProject : ICreateProject
    {
        private readonly IProjectRepository _projectRepository;

        public CreateProject(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project> ExecuteAsync(ProjectDTO projectDTO)
        {
            var project = new Project
            {

                Name = projectDTO.Name,
                Description = projectDTO.Description,
                EndDate = projectDTO.EndDate,

            };

            await this._projectRepository.Add(project);
            return project;
        }
    }
}
