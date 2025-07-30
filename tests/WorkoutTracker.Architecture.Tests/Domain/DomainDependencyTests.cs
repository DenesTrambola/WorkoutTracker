using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;
using WorkoutTracker.Domain;

namespace WorkoutTracker.Architecture.Tests.Domain;

public class DomainDependencyTests
{
    private readonly Assembly _domainAssembly = DomainAssemblyReference.Assembly;

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var otherNamespaces = new[]
        {
            RootNamespaces.ApplicationNamespace,
            RootNamespaces.PersistenceNamespace,
            RootNamespaces.InfrastructureNamespace,
            RootNamespaces.WebPresentationNamespace,
            RootNamespaces.WebHostNamespace
        };

        // Act
        var testResult = Types.InAssembly(_domainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny()
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue($"Domain assembly should not depend on any project");
    }
}
