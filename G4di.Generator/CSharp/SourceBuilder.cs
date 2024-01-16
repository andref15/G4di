using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace G4di.Generator.CSharp;
internal class SourceBuilder(LanguageVersion languageVersion)
{
    private readonly StringBuilder _stringBuilder = new();
    private readonly LanguageVersion _languageVersion = languageVersion;
    private int _indentCounter = 0;

    public SourceBuilder AppendLine()
    {
        _stringBuilder.AppendLine();
        return this;
    }

    public SourceBuilder AppendLine(string line)
    {
        AppendIndentation();
        _stringBuilder.Append(line).AppendLine(";");

        return this;
    }

    public SourceBuilder AppendNamespace(string @namespace)
    {
        _stringBuilder.Append("namespace ").Append(@namespace).AppendLine(";");

        return this;
    }

    public SourceBuilderBlock BeginClass(string modifiers, string className)
    {
        AppendIndentation();

        _stringBuilder.AppendLine($"{modifiers} class {className}");

        return new(this);
    }

    public SourceBuilderBlock BeginMethod(string modifiers, string returnType, string methodName, string? parameters = null)
    {
        AppendIndentation();
        _stringBuilder.AppendLine($"{modifiers} {returnType} {methodName}({parameters})");

        return new(this);
    }

    public SourceBuilder Using(params string[] usings)
        => Using((IEnumerable<string>)usings);

    public SourceBuilder Using(IEnumerable<string> usings)
    {
        foreach (var @using in usings)
        {
            _stringBuilder.AppendLine($"using {@using};");
        }

        return this;
    }

    private void AppendIndentation()
        => _stringBuilder.Append(string.Empty.PadRight(_indentCounter, '\t'));

    public override string ToString()
        => _stringBuilder.ToString();

    public SourceText ToSourceText()
        => SourceText.From(ToString(), Encoding.UTF8);

    public class SourceBuilderBlock : IDisposable
    {
        private readonly SourceBuilder _sourceBuilder;

        public SourceBuilderBlock(SourceBuilder sourceBuilder)
        {
            sourceBuilder.AppendIndentation();
            sourceBuilder._stringBuilder.AppendLine("{");
            sourceBuilder._indentCounter++;
            _sourceBuilder = sourceBuilder;
        }

        public void Dispose()
        {
            _sourceBuilder._indentCounter--;
            _sourceBuilder.AppendIndentation();
            _sourceBuilder._stringBuilder.AppendLine("}");
        }
    }
}
