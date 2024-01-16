namespace G4di.Generator;

[Generator(LanguageNames.CSharp)]
public class G4diGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var configFile = context.AdditionalTextsProvider
            .Where(static f => Path.GetFileName(f.Path).Equals(Configuration.FileName, StringComparison.OrdinalIgnoreCase))
            .Select(static (at, ct) => at.GetText(ct)?.ToString());
    }
}
