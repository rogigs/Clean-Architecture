using Xunit;
using System;
using Clean_Architecture.Domain.Entities;
using Bogus;
using FluentAssertions;

namespace Clean_Architecture.Tests.Domain.Entities
{

    public class ProjectTest
    {
    private readonly Faker _faker = new("pt_BR");
        [Fact]
        public void Constructor_WhenInitializedWithAttributesRequired_ShouldCreateInstance()
        {
            // Expected
            var name = _faker.Company.CompanyName();

            // Act
            Project project = new() { Name = name };

            // Assert
            project.Should().NotBeNull(); 
            project.ProjectId.Should().NotBe(Guid.Empty, "ProjectId should be initialized with a valid Guid");
            project.Name.Should().Be(name, "Name should match the provided value");
            project.Description.Should().BeNull("Description should be null if not provided");
            project.StartDate.Date.Should().Be(DateTime.Now.Date, "StartDate should be the current date");
            project.EndDate.Should().BeNull("EndDate should be null if not provided");
        }

        [Fact]
        public void Constructor_WhenInitializedWithAllAttributes_ShouldCreateInstance()
        {
            // Expected
            var name = _faker.Company.CompanyName() + _faker.Company.Random;
            var description = _faker.Company.CompanyName() + _faker.Commerce.Product();
            var endDate = _faker.Date.Future();

            // Act
            Project project = new() { Name = name, Description = description, EndDate = endDate };

            // Assert
            project.Should().NotBeNull();
            project.ProjectId.Should().NotBe(Guid.Empty, "ProjectId should be initialized with a valid Guid");
            project.Name.Should().Be(name, "Name should match the provided value");
            project.Description.Should().Be(description, "Description should match the provided value");
            project.StartDate.Date.Should().Be(DateTime.Now.Date, "StartDate should be the current date");
            project.EndDate.Should().Be(endDate, "EndDate should match the provided date");
        }
    }
}
