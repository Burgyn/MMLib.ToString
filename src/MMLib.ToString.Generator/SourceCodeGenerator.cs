using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;

namespace MMLib.ToString.Generator
{
    internal static class SourceCodeGenerator
    {
        public static string Generate(ClassModel model)
        {
            var sb = new StringBuilder();

            sb.AddNamespace(model.Namespace)
                .AddClass(model.Modifier, model.Name)
                .AddToStringOverride()
                .AddStartStringInterpolation();

            AddStringFormat(model, sb);

            sb.AddQuotationMark()
                .AddSemicolon()
                .AddCloseCurlyBracket()
                .AddCloseCurlyBracket();

            return SyntaxFactory.ParseCompilationUnit(sb.ToString())
                .NormalizeWhitespace()
                .GetText()
                .ToString();
        }

        private static void AddStringFormat(ClassModel model, StringBuilder sb)
        {
            int count = model.Properties.Length;

            sb.AppendFormat("{0} ", model.Name);
            sb.AddOpenCurlyBracket()
                .AddOpenCurlyBracket();

            for (int i = 0; i < count; i++)
            {
                sb.AppendFormat("{0} = {{ {0} }}", model.Properties[i]);
                if (i < count - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.AddCloseCurlyBracket()
                .AddCloseCurlyBracket();
        }
    }
}
