﻿using Clean_Architecture.Application.Exceptions;
using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;

namespace Clean_Architecture.Application.UseCases
{
    internal sealed class ReadProjects(IProjectRepository projectRepository) : IReadProjects
    {

        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<(ProjectException?, IEnumerable<Project>?)> ExecuteAsync(PaginationDTO pagination)
        {
            try
            {
                //TODO: Add cache 
                return (null, await _projectRepository.GetAll(pagination));
            }
            catch (Exception ex)
            {
                return (new ProjectException("An error occurred while getting projects.", ex), null);
            }
        }
    }
}
