using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace G4di.Generator;

[Generator(LanguageNames.CSharp)]
public class G4diGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var configFile = context.AdditionalTextsProvider
            .Where(static f => Path.GetFileName(f.Path).Equals(Configuration.FileName, StringComparison.OrdinalIgnoreCase))
            .Select(static (at, ct) => at.GetText(ct)?.ToString());

        var classes = context.SyntaxProvider
            .CreateSyntaxProvider((sn, ct) => sn.IsKind(SyntaxKind.ClassDeclaration), ClassDeclarationContext.Create)
            .Where(cs => !cs.TypeSymbol.IsAbstract);

        var configAndClasses = classes.Collect().Combine(configFile.Collect()).Select((a, ct) => (ConfigFile: a.Right.FirstOrDefault(), ClassDeclaration: a.Left));

        context.RegisterSourceOutput(configAndClasses, (c, s) => ExecuteAddToDiGeneration(c, s.ConfigFile, s.ClassDeclaration));
    }

    private static void ExecuteAddToDiGeneration(SourceProductionContext context, string? configFile, ImmutableArray<ClassDeclarationContext> classDeclarations)
    {
        var config = Configuration.CreateConfig((CSharpCompilation)classDeclarations[0].Model.Compilation, configFile);

        var sourceBuilder = config.CreateSourceBuilder();
        sourceBuilder.Using(Namespaces.MicrosoftExtensionsDependencyInjection, Namespaces.MicrosoftExtensionsDependencyInjectionExtensions);

        sourceBuilder.AppendNamespace(Namespaces.MicrosoftExtensionsDependencyInjection);

        using (sourceBuilder.BeginClass("public static", "GeneratedServiceCollectionExtensions"))
        {
            if (config.JsonConfig.InjectionDefinitoions is not null and { Count: > 0 })
            {
                foreach (var injectionDefinition in config.JsonConfig.InjectionDefinitoions)
                {
                    var regex = new Regex(injectionDefinition.Value.RegexPattern);
                    var matchingClasses = classDeclarations.Where(cd => regex.IsMatch(cd.Name));

                    if (!matchingClasses.Any())
                    {
                        continue;
                    }

                    using (sourceBuilder.BeginMethod("public static", TypeNames.IServiceCollection, $"Add{injectionDefinition.Key}", $"this {TypeNames.IServiceCollection} serviceCollection"))
                    {
                        sourceBuilder.AppendLine("return serviceCollection");

                        foreach (var matchingClass in matchingClasses)
                        {

                        }

                        sourceBuilder.AppendLine(";");
                    }
                }
            }
        }

        context.AddGeneratedSource("GeneratedServiceCollectionExtensions", sourceBuilder);
    }
}
