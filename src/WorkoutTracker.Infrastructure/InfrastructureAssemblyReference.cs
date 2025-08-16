using System.Reflection;

namespace WorkoutTracker.Infrastructure;

public static class InfrastructureAssemblyReference
{
    public static Assembly Assembly => typeof(InfrastructureAssemblyReference).Assembly;
}
