﻿using FluentAssertions;
using MMLib.ToString.Generator;
using Xunit;

namespace MMLib.ToString.Tests
{
    public class SourceCodeGeneratorShould
    {
        private const string ExpressionKeyWord = "##Expression##";
        private const string VersionKeyWord = "##Version##";
        private const string ClassTemplate = @"﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

/// <summary>
/// Generated partial class for overriding ToString method.
/// </summary>
namespace MMLib.Example
{
    [GeneratedCode(""MMLib.ToString"", ""##Version##"")]
    [CompilerGenerated]
    public partial class Foo
    {
        /// <summary>
        /// Generated ToString override.
        /// </summary>
        public override string ToString()
            => $""##Expression##"";
    }
}";

        [Fact]
        public void GeneratePartialClassWithToStringOverrload()
        {
            ClassModel classModel = new("MMLib.Example", "Foo", "public partial", new string[] { "Id", "Name" });

            string code = SourceCodeGenerator.Generate(classModel);
            string expected = ClassTemplate
                .Replace(VersionKeyWord, typeof(ClassModel).Assembly.GetName().Version.ToString(3))
                .Replace(ExpressionKeyWord, "Foo {{Id = {Id}, Name = {Name}}}");

            code.Should().Be(expected);
        }
    }
}
