using System.Reflection;

namespace WorkoutTracker.Persistence;

public static class PersistenceAssemblyReference
{
    public static Assembly Assembly => typeof(PersistenceAssemblyReference).Assembly;
}
