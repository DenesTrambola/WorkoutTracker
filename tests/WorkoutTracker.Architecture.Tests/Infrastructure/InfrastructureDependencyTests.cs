using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;
using WorkoutTracker.Infrastructure;

namespace WorkoutTracker.Architecture.Tests.Infrastructure;

public class InfrastructureDependencyTests
{
    private readonly Assembly _infrastructureAssembly = InfrastructureAssemblyReference.Assembly;

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var otherNamespaces = new[]
        {
            RootNamespaces.PersistenceNamespace,
            RootNamespaces.WebPresentationNamespace,
            RootNamespaces.WebHostNamespace
        };

        // Act
        var testResult = Types.InAssembly(_infrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherNamespaces)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue(
            $"Infrastructure assembly should not depend on projects: {string.Join(',', otherNamespaces)}");
    }
}
