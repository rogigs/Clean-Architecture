using Bogus;
using Projects.Application.UseCases;
using Projects.Domain.Entities;
using Projects.Domain.Interfaces;
using NSubstitute;
using FluentAssertions;
using Projects.Application.Exceptions;
using NSubstitute.ExceptionExtensions;
using Projects.Application.UseCases.DTO;

namespace Projects.Tests.Application.UseCases
{
    public class ReadProjectsTest
    {
        private readonly Faker _faker = new("pt_BR");
        private readonly IProjectRepository _projectRepositoryMock;
        private readonly ReadProjects _readProjects;

        public ReadProjectsTest()
        {
            _projectRepositoryMock = Substitute.For<IProjectRepository>();
            _readProjects = new ReadProjects(_projectRepositoryMock);
        }

        [Fact]
        public async Task ExecuteAsync_WhenArgContainPagination_ShouldGetPaginatedProjects()
        {
            // Expected
            List<Project> projects =
                [
                    new()
                    {
                        Name = _faker.Commerce.ProductName(),
                    },
                    new()
                    {
                        Name = _faker.Commerce.ProductName(),

                    }
                ];


            // Act
            PaginationDTO pagination = new() { Take = 10, Skip = 0 };
            _projectRepositoryMock.GetAll(pagination).Returns(projects);
            var (error, result) = await _readProjects.ExecuteAsync(pagination);

            // Assert
            error.Should().BeNull();
            result.Should().NotBeNull();
            result!.Should().BeEquivalentTo(projects);

            await _projectRepositoryMock.Received(1).GetAll(Arg.Is<PaginationDTO>(p =>
                p.Take == pagination.Take &&
                p.Skip == pagination.Skip
            ));
        }

        [Fact]
        public async Task ExecuteAsync_WhenErrorOccursInMethodGetAllOfProjectRepository_ShouldReturnATupleWithProjectExceptionAndProjectNull()
        {
            // Act
            PaginationDTO pagination = new() { Take = 10, Skip = 0 };

            _projectRepositoryMock
             .GetAll(Arg.Any<PaginationDTO>())
             .Throws(new Exception("Database error"));
            var (error, result) = await _readProjects.ExecuteAsync(pagination);

            // Assert
            error.Should().NotBeNull();
            error!.Should().BeOfType<ProjectException>();
            error!.Message.Should().Be("An error occurred while getting projects.");
            error!.InnerException!.Message.Should().Be("Database error");
            result.Should().BeNull();
        }
    }
}
