using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;
using WorkoutTracker.Persistence;

namespace WorkoutTracker.Architecture.Tests.Persistence;

public class PersistenceDependencyTests
{
    private readonly Assembly _persistenceAssembly = PersistenceAssemblyReference.Assembly;

    [Fact]
    public void Persistence_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var otherNamespaces = new[]
        {
            RootNamespaces.InfrastructureNamespace,
            RootNamespaces.WebPresentationNamespace,
            RootNamespaces.WebHostNamespace
        };

        // Act
        var testResult = Types.InAssembly(_persistenceAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherNamespaces)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue(
            $"Persistence assembly should not depend on projects: {string.Join(',', otherNamespaces)}");
    }
}
