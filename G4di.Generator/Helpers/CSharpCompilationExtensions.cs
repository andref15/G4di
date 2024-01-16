namespace G4di.Generator.Helpers;
internal static class CSharpCompilationExtensions
{
    public static bool IsNullableEnabled(this CSharpCompilation compilation)
        => compilation.Options.NullableContextOptions is not NullableContextOptions.Disable;
}
