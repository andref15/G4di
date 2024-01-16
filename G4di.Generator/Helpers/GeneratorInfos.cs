using System.Reflection;

namespace G4di.Generator.Helpers;
internal static class GeneratorInfos
{
    internal static string GeneratorName { get; } = typeof(G4diGenerator).FullName;
    internal static string GeneratorVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();
}
