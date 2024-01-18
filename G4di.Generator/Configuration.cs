using System.Text.Json;

namespace G4di.Generator;
internal partial class Configuration
{
    public const string FileName = "G4mvc.json";
    
    public LanguageVersion LanguageVersion { get; private set; }
    public JsonConfigClass JsonConfig { get; private set; } = null!;

    internal static Configuration CreateConfig(CSharpCompilation compilation, string? configFile)
    {
        Configuration configuration = new()
        {
            LanguageVersion = compilation.LanguageVersion,
            JsonConfig = configFile is null ? new() : JsonSerializer.Deserialize<JsonConfigClass>(configFile) ?? new()
        };

        return configuration;
    }

    internal static Configuration CreateConfig(CSharpParseOptions parseOptions, string? configFile)
    {
        Configuration configuration = new()
        {
            LanguageVersion = parseOptions.LanguageVersion,
            JsonConfig = configFile is null ? new() : JsonSerializer.Deserialize<JsonConfigClass>(configFile) ?? new()
        };

        return configuration;
    }

    internal SourceBuilder CreateSourceBuilder()
        => new(LanguageVersion);

    internal class JsonConfigClass
    {
        public Dictionary<string, RegexInjectionDefinitoion>? InjectionDefinitoions { get; set; }
    }
}
