using System.Text;

namespace MMLib.ToString.Generator
{
    internal static class StringBuilderExtensions
    {
        private const char OpenCurlyBracket = '{';
        private const char CloseCurlyBracket = '}';

        public static StringBuilder AddOpenCurlyBracket(this StringBuilder sb)
            => sb.Append(OpenCurlyBracket);

        public static StringBuilder AddCloseCurlyBracket(this StringBuilder sb)
            => sb.Append(CloseCurlyBracket);

        public static StringBuilder AddNamespace(this StringBuilder sb, string namespaceName)
            => sb.AppendFormat("namespace {0}", namespaceName)
                .AddOpenCurlyBracket();

        public static StringBuilder AddClass(this StringBuilder sb, string modifiers, string name)
            => sb.AppendFormat("{0} class {1}", modifiers, name)
                .AddOpenCurlyBracket();

        public static StringBuilder AddToStringOverride(this StringBuilder sb)
            => sb.AppendLine("public override string ToString()")
                .AppendLine("=>");

        public static StringBuilder AddQuotationMark(this StringBuilder sb)
            => sb.Append('"');

        public static StringBuilder AddStartStringInterpolation(this StringBuilder sb)
            => sb.Append('$').AddQuotationMark();

        public static StringBuilder AddSemicolon(this StringBuilder sb)
            => sb.Append(';');
    }
}
