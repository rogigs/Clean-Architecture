using Bogus;
using Projects.Application.UseCases;
using Projects.Application.UseCases.DTO;
using Projects.Domain.Entities;
using Projects.Domain.Interfaces;
using NSubstitute;
using FluentAssertions;
using Projects.Application.Exceptions;
using NSubstitute.ExceptionExtensions;

namespace Projects.Tests.Application.UseCases
{
    public class UpdateProjectTest
    {
        private readonly Faker _faker = new("pt_BR");

        private readonly IProjectRepository _projectRepositoryMock;
        private readonly UpdateProject _updateProject;

        public UpdateProjectTest()
        {
            _projectRepositoryMock = Substitute.For<IProjectRepository>();
            _updateProject = new UpdateProject(_projectRepositoryMock);
        }

        [Fact]
        public async Task ExecuteAsync_WhenArgContainIdAndProject_ShouldUpdateAProjectById()
        {
            // Expected
            ProjectUpdateDTO projectUpdateDTO = new(
                 _faker.Commerce.ProductName(),
                null,
                null
            );

            Project project = new()
            {
                Name = _faker.Commerce.ProductName(),
            };

            // Act
            _projectRepositoryMock.Update(projectUpdateDTO, project.ProjectId).Returns(project);
            var (error, result) = await _updateProject.ExecuteAsync(projectUpdateDTO, project.ProjectId);

            // Assert
            error.Should().BeNull();
            result.Should().NotBeNull();
            result!.Should().Be(project);

            await _projectRepositoryMock.Received(1).Update(
                Arg.Is<ProjectUpdateDTO>(dto => dto.Name == projectUpdateDTO.Name &&
                                                  dto.Description == projectUpdateDTO.Description &&
                                                  dto.EndDate == projectUpdateDTO.EndDate),
                Arg.Is<Guid>(id => id == project.ProjectId));
        }

        [Fact]
        public async Task ExecuteAsync_WhenErrorOccursInMethodUpdateOfProjectRepository_ShouldReturnATupleWithProjectExceptionAndProjectNull()
        {
            // Expected
            ProjectUpdateDTO projectUpdateDTO = new(
                _faker.Commerce.ProductName(),
                null,
                null
            );

            Project project = new()
            {
                Name = _faker.Commerce.ProductName(),
            };

            // Act
            _projectRepositoryMock
                 .Update(Arg.Any<ProjectUpdateDTO>(), Arg.Any<Guid>())
                 .Throws(new Exception("Database error"));

            var (error, result) = await _updateProject.ExecuteAsync(projectUpdateDTO, project.ProjectId);

            // Assert
            error.Should().NotBeNull();
            error!.Should().BeOfType<ProjectException>();
            error!.Message.Should().Be("An error occurred while uploading a project.");
            error!.InnerException!.Message.Should().Be("Database error");
            result.Should().BeNull();
        }
    }
}
