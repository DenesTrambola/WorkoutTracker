using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace WorkoutTracker.Architecture.Tests.Application;

public class ApplicationDependencyTests
{
    private readonly Assembly _applicationAssembly = WorkoutTracker.Application.ApplicationAssemblyReference.Assembly;

    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var otherNamespaces = new[]
        {
            RootNamespaces.PersistenceNamespace,
            RootNamespaces.InfrastructureNamespace,
            RootNamespaces.WebPresentationNamespace,
            RootNamespaces.WebHostNamespace
        };

        // Act
        var testResult = Types.InAssembly(_applicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherNamespaces)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue(
            $"Application assembly should not depend on projects: {string.Join(',', otherNamespaces)}");
    }
}
