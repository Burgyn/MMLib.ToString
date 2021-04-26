using FluentAssertions;
using MMLib.ToString.Generator;
using Xunit;

namespace MMLib.ToString.Tests
{
    public class SourceCodeGeneratorShould
    {
        private const string ExpressionKeyWord = "##Expression##";
        private const string ClassTemplate = @"namespace MMLib.Example
{
    public partial class Foo
    {
        public override string ToString() => $""##Expression##"";
    }
}";
        [Fact]
        public void GeneratePartialClassWithToStringOverrload()
        {
            ClassModel classModel = new("MMLib.Example", "Foo", "public partial", new string[] { "Id", "Name" });

            string code = SourceCodeGenerator.Generate(classModel);
            string expected = ClassTemplate.Replace(ExpressionKeyWord, "Foo {{Id = {Id}, Name = {Name}}}");

            code.Should().Be(expected);
        }
    }
}
