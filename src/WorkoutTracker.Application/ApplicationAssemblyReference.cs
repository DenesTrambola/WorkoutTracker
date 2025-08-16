using System.Reflection;

namespace WorkoutTracker.Application;

public static class ApplicationAssemblyReference
{
    public static Assembly Assembly => typeof(ApplicationAssemblyReference).Assembly;
}
