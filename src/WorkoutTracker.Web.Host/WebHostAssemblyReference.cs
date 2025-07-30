using System.Reflection;

namespace WorkoutTracker.Web.Host;

public static class WebHostAssemblyReference
{
    public static Assembly Assembly => typeof(WebHostAssemblyReference).Assembly;
}
