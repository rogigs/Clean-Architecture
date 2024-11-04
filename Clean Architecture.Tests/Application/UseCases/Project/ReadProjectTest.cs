using Bogus;
using Clean_Architecture.Application.UseCases;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;
using NSubstitute;
using FluentAssertions;
using Clean_Architecture.Application.Exceptions;
using NSubstitute.ExceptionExtensions;

namespace Clean_Architecture.Tests.Application.UseCases
{
    public class ReadProjectTest
    {
        private readonly Faker _faker = new("pt_BR");
        private readonly IProjectRepository _projectRepositoryMock;
        private readonly ReadProject _readProject;

        public ReadProjectTest()
        {
            _projectRepositoryMock = Substitute.For<IProjectRepository>();
            _readProject = new ReadProject(_projectRepositoryMock);
        }

        [Fact]
        public async Task ExecuteAsync_WhenArgContainId_ShouldGetAProjectById()
        {
            // Expected
            Project project = new()
            {
                Name = _faker.Commerce.ProductName(),
            };

            // Act
            _projectRepositoryMock.GetById(project.ProjectId).Returns(project);
            var (error, result) = await _readProject.ExecuteAsync(project.ProjectId);

            // Assert
            error.Should().BeNull();
            result.Should().NotBeNull();
            result!.Should().Be(project);

            await _projectRepositoryMock.Received(1).GetById(Arg.Is<Guid>(id => id == project.ProjectId));
        }

        [Fact]
        public async Task ExecuteAsync_WhenErrorOccursInMethodGetByIdOfProjectRepository_ShouldReturnATupleWithProjectExceptionAndProjectNull()
        {
            // Expected
            Project project = new()
            {
                Name = _faker.Commerce.ProductName(),
            };

            // Act
            _projectRepositoryMock
         .GetById(Arg.Any<Guid>())
         .Throws(new Exception("Database error"));
            var (error, result) = await _readProject.ExecuteAsync(project.ProjectId);

            // Assert
            error.Should().NotBeNull();
            error!.Should().BeOfType<ProjectException>();
            error!.Message.Should().Be("An error occurred while getting a project.");
            error!.InnerException!.Message.Should().Be("Database error");
            result.Should().BeNull();
        }
    }
}
