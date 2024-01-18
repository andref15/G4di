using G4di.TestBase;
using G4di.TestBase.Asserts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace G4di.Test_8;

[TestClass]
public class G4diTest : G4diTestBase
{
    public G4diTest() : base(LanguageVersion.CSharp12)
    {
    }

    [TestMethod]
    public void TestMethod1()
    {
        var compilation = BaseTest();

        var expectedOutputs = new ExpectedOutput();
        
    }
}