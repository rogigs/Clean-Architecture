using System;
using Bogus;
using Clean_Architecture.Domain.Interfaces;
using Clean_Architecture.Application.UseCases;
using NSubstitute;
using Clean_Architecture.Application.UseCases.DTO;
using FluentAssertions;
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
            var projectDTO = new ProjectDTO(
           _faker.Commerce.ProductName(),
           _faker.Lorem.Paragraph(),      
           _faker.Date.Future()          
       );
            // Act
            var result = await _createProject.ExecuteAsync(projectDTO);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(projectDTO.Name);
            result.Description.Should().Be(projectDTO.Description);
            result.EndDate.Should().Be(projectDTO.EndDate);

            await _projectRepositoryMock.Received(1).Add(Arg.Is<Project>(p =>
                p.Name == projectDTO.Name &&
                p.Description == projectDTO.Description &&
                p.EndDate == projectDTO.EndDate));
        }

        [Fact]
        public async Task ExecuteAsync_WhenArgContainJustRequiredAttributes_ShouldCreateAProject()
        {
            // Expected
            var projectDTO = new ProjectDTO(
          _faker.Commerce.ProductName(), null, null
      );
            // Act
            var result = await _createProject.ExecuteAsync(projectDTO);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(projectDTO.Name);


            await _projectRepositoryMock.Received(1).Add(Arg.Is<Project>(p =>
                p.Name == projectDTO.Name));
        }

        //TODO: add Exception
        //[Fact]
        //public async Task ExecuteAsync_WhenArgRequiredIsNull_ShouldThrowArgumentNullException()
        //{
        //    // Expected
        //    var projectDTO = new ProjectDTO(
        //        null, 
        //        null,
        //        null
        //    );

        //    // Act
        //    Func<Task> act = async () => await _createProject.ExecuteAsync(projectDTO);

        //    // Assert
        //    await act.Should().ThrowAsync<ArgumentNullException>()
        //        .WithMessage("Expected a <System.ArgumentNullException> to be thrown, but no exception was thrown.");
        //}
    }
}
