using Bogus;
using Clean_Architecture.Application.UseCases;
using Clean_Architecture.Domain.Interfaces;
using NSubstitute;
using FluentAssertions;
using Clean_Architecture.Domain.Entities;
using Xunit.Abstractions;
using Clean_Architecture.Application.Exceptions;
using NSubstitute.ExceptionExtensions;

namespace Clean_Architecture.Tests.Application.UseCases
{
    public class DeleteProjectTest
    {

        private readonly Faker _faker = new("pt_BR");
        private readonly ITestOutputHelper _output;

        private readonly IProjectRepository _projectRepositoryMock;
        private readonly DeleteProject _deleteProject;

        public DeleteProjectTest(ITestOutputHelper output)
        {
            _projectRepositoryMock = Substitute.For<IProjectRepository>();
            _deleteProject = new DeleteProject(_projectRepositoryMock);
            _output = output;

        }

        [Fact]
        public async Task ExecuteAsync_WhenArgContainId_ShouldDeleteAProject()
        {
            // Expected
            var project = new Project
            {
                Name = _faker.Commerce.ProductName(),
            };

            // Act
            _projectRepositoryMock.Delete(project.ProjectId).Returns(project);
            var (error, result) = await _deleteProject.ExecuteAsync(project.ProjectId);

            _output.WriteLine($"Expected: {project}, Result: {result}");

            // Assert
            error.Should().BeNull();
            result.Should().NotBeNull();
            result!.Should().Be(project);


            await _projectRepositoryMock.Received(1).Delete(Arg.Is<Guid>(id => id == project.ProjectId));
        }

        [Fact]
        public async Task ExecuteAsync_WhenErrorOccursInMethodAddOfProjectRepository_ShouldReturnATupleWithProjectExceptionAndProjectNull()
        {
            // Expected
            var project = new Project
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
            error!.Message.Should().Be("An error occurred while deleting the project.");
            error!.InnerException!.Message.Should().Be("Database error");
            result.Should().BeNull();
        }
    }
}
