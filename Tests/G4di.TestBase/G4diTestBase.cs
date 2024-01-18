using G4di.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace G4di.TestBase;

public abstract class G4diTestBase(LanguageVersion languageVersion)
{
    protected readonly CSharpCompilationOptions CompilationOptions = new(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release, warningLevel: 0, nullableContextOptions: NullableContextOptions.Enable);

    protected readonly CSharpParseOptions ParseOptions = CSharpParseOptions.Default.WithLanguageVersion(languageVersion).WithDocumentationMode(DocumentationMode.None);

    private protected CSharpCompilation BaseTest(Configuration.JsonConfigClass? jsonConfig = null, IEnumerable<SyntaxTree>? additionalSyntaxTrees = null)
    {
        var syntaxTrees = GetSyntaxTreesInBuildDirectory();

        if (additionalSyntaxTrees is not null)
        {
            syntaxTrees = syntaxTrees.Concat(additionalSyntaxTrees);
        }

        var g4mvcGenerator = new G4diGenerator();

        var compilation = CSharpCompilation.Create("TestLib", syntaxTrees, GetMetadataReferences(), options: CompilationOptions);

        var generatorDriver = CSharpGeneratorDriver.Create(g4mvcGenerator).WithUpdatedParseOptions(ParseOptions);

        if (jsonConfig is not null)
        {
            generatorDriver = generatorDriver.AddAdditionalTexts([new ConfigAdditionalText(jsonConfig)]);
        }

        generatorDriver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilationBase, out _);

        Assert.IsInstanceOfType<CSharpCompilation>(outputCompilationBase);
        var outputCompilation = (CSharpCompilation)outputCompilationBase;

        AssertDiagnostics(outputCompilation, "Output");

        return outputCompilation;
    }

    protected IEnumerable<SyntaxTree> GetSyntaxTreesInBuildDirectory()
    {
        var directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);

        foreach (var file in directoryInfo.EnumerateFiles("*.cs", SearchOption.AllDirectories).OrderBy(f => f.Name))
        {
            using var stream = file.OpenRead();
            var sourceText = SourceText.From(stream);
            yield return SyntaxFactory.ParseSyntaxTree(sourceText, ParseOptions);
        }
    }

    protected static IEnumerable<MetadataReference> GetMetadataReferences()
    {
        var msAssemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location)!;

        yield return MetadataReference.CreateFromFile(Path.Combine(msAssemblyPath, "mscorlib.dll"));
        yield return MetadataReference.CreateFromFile(Path.Combine(msAssemblyPath, "System.dll"));
        yield return MetadataReference.CreateFromFile(Path.Combine(msAssemblyPath, "System.Core.dll"));
        yield return MetadataReference.CreateFromFile(Path.Combine(msAssemblyPath, "System.Runtime.dll"));

        yield return MetadataReference.CreateFromFile(typeof(System.Runtime.JitInfo).Assembly.Location);
        yield return MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);
        yield return MetadataReference.CreateFromFile(typeof(System.Text.Json.JsonSerializer).Assembly.Location);
    }

    protected static void AssertDiagnostics(CSharpCompilation compilation, string type)
    {
        var diagnostics = compilation.GetDiagnostics();
        Assert.AreEqual(0, diagnostics.Length, "{0} classes have diagnostic messages:\n{1}", type, string.Join('\n', diagnostics));
    }
}