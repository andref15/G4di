using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace G4di.TestBase.Asserts;
public class SyntaxAssert(SyntaxTree expected)
{
    private SyntaxTree _expected = expected;

    public void IsEquivalentTo(SyntaxTree actual)
        => IsEquivalentTo(actual, false);

    public void IsEquivalentTo(SyntaxTree actual, string message)
        => IsEquivalentTo(actual, false, message);

    public void IsEquivalentTo(SyntaxTree actual, bool topLevel)
        => IsEquivalentTo(actual, topLevel, "Expected syntax did not match actual syntax!");

    public void IsEquivalentTo(SyntaxTree actual, bool topLevel, string message)
    {
        if (!_expected.IsEquivalentTo(actual, topLevel))
        {
            ThrowAssertFailed(nameof(IsEquivalentTo), message);
        }
    }

    public void AreAnyEquivalentTo(IEnumerable<SyntaxTree> actual)
        => AreAnyEquivalentTo(actual, false);

    public void AreAnyEquivalentTo(IEnumerable<SyntaxTree> actual, string message)
        => AreAnyEquivalentTo(actual, false, message);

    public void AreAnyEquivalentTo(IEnumerable<SyntaxTree> actual, bool topLevel)
        => AreAnyEquivalentTo(actual, topLevel, "No syntax trees match the expected syntax!");

    public void AreAnyEquivalentTo(IEnumerable<SyntaxTree> actual, bool topLevel, string message)
    {
        if (!actual.Any(a => _expected.IsEquivalentTo(a, topLevel)))
        {
            ThrowAssertFailed(nameof(IsEquivalentTo), message);
        }
    }

    [DoesNotReturn]
    private void ThrowAssertFailed(string method, string? message)
        => throw new AssertFailedException($"{nameof(SyntaxAssert)}.{method} failed! {message}");
}
