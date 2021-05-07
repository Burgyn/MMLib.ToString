using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MMLib.ToString.Abstraction;
using System.Collections.Generic;

namespace MMLib.ToString.Generator
{
    public sealed class ToStringReceiver: ISyntaxReceiver
    {
        private static readonly string _attributeShort = nameof(ToStringAttribute).TrimEnd("Attribute");
        private readonly List<ClassDeclarationSyntax> _candidates = new();

        public IEnumerable<ClassDeclarationSyntax> Candidates => _candidates;

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classSyntax && classSyntax.HaveAttribute(_attributeShort))
            {
                _candidates.Add(classSyntax);
            }
        }
    }
}
