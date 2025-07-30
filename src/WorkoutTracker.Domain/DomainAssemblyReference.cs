using System.Reflection;

namespace WorkoutTracker.Domain;

public static class DomainAssemblyReference
{
    public static Assembly Assembly => typeof(DomainAssemblyReference).Assembly;
}
