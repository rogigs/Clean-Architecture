using Bogus;
using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;
using Clean_Architecture.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using FluentAssertions;
using Clean_Architecture.Application.Exceptions;

namespace Clean_Architecture.Tests.Web.Controllers
{
    public class ProjectControllerTest
    {

        private readonly Faker _faker = new("pt_BR");

        private readonly ProjectController _controller;
        private readonly IProjectRepository _projectRepositoryMock;
        private readonly ICreateProject _createProject;
        private readonly IReadProject _readProject;
        private readonly IReadProjects _readProjects;
        private readonly IDeleteProject _deleteProject;
        private readonly IUpdateProject _updateProject;


        public ProjectControllerTest()
        {
            _projectRepositoryMock = Substitute.For<IProjectRepository>();
            _createProject = Substitute.For<ICreateProject>();
            _readProject = Substitute.For<IReadProject>();
            _readProjects = Substitute.For<IReadProjects>();
            _deleteProject = Substitute.For<IDeleteProject>();
            _updateProject = Substitute.For<IUpdateProject>();
            _controller = new ProjectController(_createProject, _readProject, _readProjects, _deleteProject, _updateProject);
        }

        [Fact]
        public async Task PostAsync_WhenCreateProjectIsSuccess_ShouldReturnAProject()
        {
            // Expected
            ProjectDTO projectDTO = new(
                _faker.Commerce.ProductName(),
                null,
                null
             );

            Project project = new() { Name = projectDTO.Name };

            // Act
            _createProject.ExecuteAsync(projectDTO).Returns((null, project));
            IActionResult result = await _controller.PostAsync(projectDTO);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>()
                .Which.Value.Should()
                    .BeEquivalentTo(project, options => options
                    .Excluding(p => p.ProjectId)
                    .Excluding(p => p.StartDate)
            );
        }

        [Fact]
        public async Task PostAsync_WhenCreateProjectIsError_ShouldReturnBadRequest()
        {
            // Expected
            ProjectDTO projectDTO = new(
                _faker.Commerce.ProductName(),
                null,
                null
             );
            ProjectException error = new("An error occurred while deleting a project.");

            // Act
            _createProject.ExecuteAsync(projectDTO).Returns((error, null));
            IActionResult result = await _controller.PostAsync(projectDTO);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { error.Message });

        }



    }
}
