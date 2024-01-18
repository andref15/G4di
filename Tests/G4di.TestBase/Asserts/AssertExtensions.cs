using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G4di.TestBase.Asserts;
public static class AssertExtensions
{
    public static SyntaxAssert Syntax(this Assert _, SyntaxTree expected)
        => new(expected);
}
