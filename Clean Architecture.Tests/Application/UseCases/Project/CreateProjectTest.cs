using Bogus;
using Clean_Architecture.Domain.Interfaces;
using Clean_Architecture.Application.UseCases;
using NSubstitute;
using Clean_Architecture.Application.UseCases.DTO;
using FluentAssertions;
using Clean_Architecture.Application.Exceptions;
using NSubstitute.ExceptionExtensions;
using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Tests.Application.UseCases
{
    public class CreateProjectTest
    {
        private readonly Faker _faker = new("pt_BR");

        private readonly IProjectRepository _projectRepositoryMock;
        private readonly CreateProject _createProject;

        public CreateProjectTest()
        {
            _projectRepositoryMock = Substitute.For<IProjectRepository>();
            _createProject = new CreateProject(_projectRepositoryMock);
        }

        [Fact]
        public async Task ExecuteAsync_WhenArgContainAllAtributes_ShouldCreateAProject()
        {
            // Expected
            ProjectDTO projectDTO = new (
           _faker.Commerce.ProductName(),
           _faker.Lorem.Paragraph(),
           _faker.Date.Future()
       );
            // Act
            var (error, result) = await _createProject.ExecuteAsync(projectDTO);

            // Assert
            error.Should().BeNull();
            result.Should().NotBeNull();
            result!.Name.Should().Be(projectDTO.Name);
            result!.Description.Should().Be(projectDTO.Description);
            result!.EndDate.Should().Be(projectDTO.EndDate);

            await _projectRepositoryMock.Received(1).Add(Arg.Is<Project>(p =>
                p.Name == projectDTO.Name &&
                p.Description == projectDTO.Description &&
                p.EndDate == projectDTO.EndDate));
        }

        [Fact]
        public async Task ExecuteAsync_WhenArgContainJustRequiredAttributes_ShouldCreateAProject()
        {
            // Expected
            ProjectDTO projectDTO = new (
          _faker.Commerce.ProductName(), null, null
      );
            // Act
            var (error, result) = await _createProject.ExecuteAsync(projectDTO);

            // Assert
            error.Should().BeNull();
            result.Should().NotBeNull();
            result!.Name.Should().Be(projectDTO.Name);

            await _projectRepositoryMock.Received(1).Add(Arg.Is<Project>(p =>
                p.Name == projectDTO.Name));
        }

        [Fact]
        public async Task ExecuteAsync_WhenErrorOccursInMethodAddOfProjectRepository_ShouldReturnATupleWithProjectExceptionAndProjectNull()
        {
            // Expected
            ProjectDTO projectDTO = new (
             _faker.Commerce.ProductName(), null, null
         );

            // Act
            _projectRepositoryMock
         .Add(Arg.Any<Project>())
         .Throws(new Exception("Database error"));
            var (error, result) = await _createProject.ExecuteAsync(projectDTO);

            // Assert
            error.Should().NotBeNull();
            error!.Should().BeOfType<ProjectException>();
            error!.Message.Should().Be("An error occurred while creating a project.");
            error!.InnerException!.Message.Should().Be("Database error");
            result.Should().BeNull();
        }
    }
}
