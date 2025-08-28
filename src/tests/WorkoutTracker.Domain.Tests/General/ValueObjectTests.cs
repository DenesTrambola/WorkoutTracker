namespace WorkoutTracker.Domain.Tests.General;

using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions;
using WorkoutTracker.Domain.Shared.Primitives;

public sealed class ValueObjectTests
{
    private readonly Assembly domainAssembly = DomainAssemblyReference.Assembly;

    [Fact]
    public void ValueObjects_Should_InheritFromValueObjectBaseClass()
    {
        var valueObjectTypes = domainAssembly
            .GetTypes()
            .Where(static t =>
                t.IsClass
                && !t.IsAbstract
                && t.Namespace is not null
                && t.Namespace.EndsWith("ValueObjects", StringComparison.Ordinal)
                && !t.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false)
                && !t.Name.Contains("<>", StringComparison.Ordinal));

        foreach (var vo in valueObjectTypes)
        {
            vo.IsSubclassOf(typeof(ValueObject))
                .Should().BeTrue();
        }
    }
}
