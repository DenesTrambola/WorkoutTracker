using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;
using WorkoutTracker.Web.Presentation;

namespace WorkoutTracker.Architecture.Tests.Web.Presentation;

public class WebPresentationDependencyTests
{
    private readonly Assembly _webPresentationAssembly = WebPresentationAssemblyReference.Assembly;

    [Fact]
    public void WebPresentation_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var otherNamespaces = new[]
        {
            RootNamespaces.WebHostNamespace
        };

        // Act
        var testResult = Types.InAssembly(_webPresentationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherNamespaces)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue(
            $"Web.Presentation assembly should not depend on projects: {string.Join(',', otherNamespaces)}");
    }
}
