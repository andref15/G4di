namespace G4di.Generator.Helpers;
internal static class TypeSymbolExtensions
{
    internal static bool IsTypeOrInterface(this ITypeSymbol typeSymbol, string type)
        => typeSymbol.ToDisplayString() == type;

    internal static bool DerrivesFromType(this ITypeSymbol typeSymbol, string type)
        => typeSymbol.IsTypeOrInterface(type) || typeSymbol.BaseType is not null && typeSymbol.BaseType.DerrivesFromType(type);

    internal static bool IsOrImplementsInterface(this ITypeSymbol typeSymbol, string @interface)
        => typeSymbol.ToDisplayString() == @interface || typeSymbol.AllInterfaces.Any(i => i.ToDisplayString() == @interface);
}
