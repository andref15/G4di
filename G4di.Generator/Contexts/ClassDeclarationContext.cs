namespace G4di.Generator.Contexts;
internal class ClassDeclarationContext
{
    public SemanticModel Model { get; }
    public ClassDeclarationSyntax Syntax { get; }
    public INamedTypeSymbol TypeSymbol { get; }
    public string Name { get; }

    private ClassDeclarationContext(SemanticModel model, ClassDeclarationSyntax syntax, INamedTypeSymbol typeSymbol)
    {
        Model = model;
        Syntax = syntax;
        TypeSymbol = typeSymbol;

        Name = typeSymbol.Name;
    }

    public static ClassDeclarationContext Create(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        return new ClassDeclarationContext(context.SemanticModel, classDeclaration, context.SemanticModel.GetDeclaredSymbol(classDeclaration, cancellationToken)!);
    }
}
