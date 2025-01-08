using Bogus;
using Projects.Application.UseCases;
using Projects.Domain.Interfaces;
using NSubstitute;
using FluentAssertions;
using Projects.Domain.Entities;
using Xunit.Abstractions;
using Projects.Application.Exceptions;
using NSubstitute.ExceptionExtensions;

namespace Projects.Tests.Application.UseCases
{
    public class DeleteProjectTest
    {

        private readonly Faker _faker = new("pt_BR");
        private readonly IProjectRepository _projectRepositoryMock;
        private readonly DeleteProject _deleteProject;

        public DeleteProjectTest()
        {
            _projectRepositoryMock = Substitute.For<IProjectRepository>();
            _deleteProject = new DeleteProject(_projectRepositoryMock);
        }

        [Fact]
        public async Task ExecuteAsync_WhenArgContainId_ShouldDeleteAProject()
        {
            // Expected
            Project project = new()
            {
                Name = _faker.Commerce.ProductName(),
            };

            // Act
            _projectRepositoryMock.Delete(project.ProjectId).Returns(project);
            var (error, result) = await _deleteProject.ExecuteAsync(project.ProjectId);

            // Assert
            error.Should().BeNull();
            result.Should().NotBeNull();
            result!.Should().Be(project);


            await _projectRepositoryMock.Received(1).Delete(Arg.Is<Guid>(id => id == project.ProjectId));
        }

        [Fact]
        public async Task ExecuteAsync_WhenErrorOccursInMethodDeleteOfProjectRepository_ShouldReturnATupleWithProjectExceptionAndProjectNull()
        {
            // Expected
            Project project = new()
            {
                Name = _faker.Commerce.ProductName(),
            };


            // Act
            _projectRepositoryMock
         .Delete(Arg.Any<Guid>())
         .Throws(new Exception("Database error"));
            var (error, result) = await _deleteProject.ExecuteAsync(project.ProjectId);

            // Assert
            error.Should().NotBeNull();
            error!.Should().BeOfType<ProjectException>();
            error!.Message.Should().Be("An error occurred while deleting a project.");
            error!.InnerException!.Message.Should().Be("Database error");
            result.Should().BeNull();
        }
    }
}
