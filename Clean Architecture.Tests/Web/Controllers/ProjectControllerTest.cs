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
        protected readonly Faker _faker = new("pt_BR");

        protected readonly ProjectController _controller;
        protected readonly IProjectRepository _projectRepositoryMock;
        protected readonly ICreateProject _createProject;
        protected readonly IReadProject _readProject;
        protected readonly IReadProjects _readProjects;
        protected readonly IDeleteProject _deleteProject;
        protected readonly IUpdateProject _updateProject;

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
    }

    public class PostProjectTests : ProjectControllerTest
    {
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
            ProjectException error = new("An error occurred while creating a project.");

            // Act
            _createProject.ExecuteAsync(projectDTO).Returns((error, null));
            IActionResult result = await _controller.PostAsync(projectDTO);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { error.Message });
        }
    }

    public class GetProjectTests : ProjectControllerTest
    {
        [Fact]
        public async Task GetAsync_WhenGetAProjectByIdIsSuccess_ShouldReturnTheProject()
        {
            // Expected
            Guid projectId = Guid.NewGuid();
            Project project = new() { ProjectId = projectId, Name = _faker.Commerce.ProductName() };

            // Act
            _readProject.ExecuteAsync(projectId).Returns((null, project));
            IActionResult result = await _controller.GetAsync(projectId);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should()
                    .BeEquivalentTo(project, options => options
                    .Excluding(p => p.StartDate)
            );
        }

        [Fact]
        public async Task GetAsync_WhenGetAProjectByIdIsNotFound_ShouldReturnAMessageNotFound()
        {
            // Expected
            Guid projectId = Guid.NewGuid();

            // Act
            _readProject.ExecuteAsync(projectId).Returns((null, null));
            IActionResult result = await _controller.GetAsync(projectId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { Message = "Project not found" });
        }

        [Fact]
        public async Task GetAsync_WhenGetAProjectByIdIsError_ShouldReturnBadRequest()
        {
            // Expected
            Guid projectId = Guid.NewGuid();
            ProjectException error = new("An error occurred while getting a project.");

            // Act
            _readProject.ExecuteAsync(projectId).Returns((error, null));
            IActionResult result = await _controller.GetAsync(projectId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
               .Which.Value.Should().BeEquivalentTo(new { error.Message });
        }
    }

    public class DeleteProjectTests : ProjectControllerTest
    {
        [Fact]
        public async Task DeleteAsync_WhenDeleteAsyncIsSuccess_ShouldReturnTheProject()
        {
            // Expected
            Guid projectId = Guid.NewGuid();
            Project project = new() { ProjectId = projectId, Name = _faker.Commerce.ProductName() };

            // Act
            _deleteProject.ExecuteAsync(projectId).Returns((null, project));
            IActionResult result = await _controller.DeleteAsync(projectId);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should()
                    .BeEquivalentTo(project, options => options
                    .Excluding(p => p.StartDate)
            );
        }

        [Fact]
        public async Task DeleteAsync_WhenDeleteAsyncIsNotFound_ShouldReturnAMessageNotFound()
        {
            // Expected
            Guid projectId = Guid.NewGuid();

            // Act
            _deleteProject.ExecuteAsync(projectId).Returns((null, null));
            IActionResult result = await _controller.DeleteAsync(projectId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { Message = "Project not found" });
        }

        [Fact]
        public async Task DeleteAsync_WhenDeleteAsyncIsError_ShouldReturnBadRequest()
        {
            // Expected
            Guid projectId = Guid.NewGuid();
            ProjectException error = new("An error occurred while deleting a project.");

            // Act
            _deleteProject.ExecuteAsync(projectId).Returns((error, null));
            IActionResult result = await _controller.DeleteAsync(projectId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { error.Message });
        }
    }

    public class UpdateProjectTests : ProjectControllerTest
    {
        [Fact]
        public async Task UpdateAsync_WhenUpdateProjectByIdIsSuccess_ShouldReturnUpdatedProject()
        {
            // Expected
            Guid projectId = Guid.NewGuid();
            string name = _faker.Commerce.ProductName();

            ProjectUpdateDTO projectDTO = new(
                name,
                null,
                null
            );

            Project project = new() { ProjectId = projectId, Name = name };

            // Act
            _updateProject.ExecuteAsync(projectDTO, projectId).Returns((null, project));
            IActionResult result = await _controller.UpdateAsync(projectDTO, projectId);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should()
                    .BeEquivalentTo(project, options => options
                    .Excluding(p => p.StartDate)
            );
        }

        [Fact]
        public async Task UpdateAsync_WhenUpdateProjectByIdIsError_ShouldReturnBadRequest()
        {
            // Expected
            Guid projectId = Guid.NewGuid();
            ProjectUpdateDTO projectDTO = new(
                _faker.Commerce.ProductName(),
                null,
                null
            );
            ProjectException error = new("An error occurred while updating the project.");

            // Act
            _updateProject.ExecuteAsync(projectDTO, projectId).Returns((error, null));
            IActionResult result = await _controller.UpdateAsync(projectDTO, projectId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { error.Message });
        }

        [Fact]
        public async Task UpdateAsync_WhenUpdateProjectByIdIsNotFound_ShouldReturnAMessageNotFound()
        {
            // Expected
            Guid projectId = Guid.NewGuid();

            // Act
            _readProject.ExecuteAsync(projectId).Returns((null, null));
            IActionResult result = await _controller.GetAsync(projectId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { Message = "Project not found" });
        }
    }

    public class GetProjectsTests : ProjectControllerTest
    {
        [Fact]
        public async Task GetAllAsync_WhenListProjectsIsSuccess_ShouldReturnListWithProjects()
        {
            // Expected
            PaginationDTO pagination = new() { Take = 10, Skip = 0 };
            List<Project> projects =
             [
                 new ()
                    {
                        Name = _faker.Commerce.ProductName(),

                    },
                    new ()
                    {
                        Name = _faker.Commerce.ProductName(),

                    }
             ];

            // Act
            _readProjects.ExecuteAsync(pagination).Returns((null, projects));
            IActionResult result = await _controller.GetAllAsync(pagination.Take, pagination.Skip);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
             .Which.Value.Should()
                 .BeEquivalentTo(projects, options => options
                 .Using<Project>(ctx => ctx.Subject.Should().BeEquivalentTo(ctx.Expectation,
                     options => options
                         .Excluding(p => p.ProjectId)
                         .Excluding(p => p.StartDate)))
                 .WhenTypeIs<Project>()); 
        }

        [Fact]
        public async Task GetAllAsync_WhenListProjectsIsError_ShouldReturnListWithProjects()
        {
            // Expected
            PaginationDTO pagination = new() { Take = 10, Skip = 0 };
            ProjectException error = new("An error occurred while getting projects.");

            // Act
            _readProjects.ExecuteAsync(pagination).Returns((error, null));
            IActionResult result = await _controller.GetAllAsync(pagination.Take, pagination.Skip);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { error.Message });
        }

    }
}