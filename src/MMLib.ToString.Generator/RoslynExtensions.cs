﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMLib.ToString.Generator
{
    internal static class RoslynExtensions
    {
        public static CompilationUnitSyntax GetCompilationUnit(this SyntaxNode syntaxNode)
            => syntaxNode.Ancestors().OfType<CompilationUnitSyntax>().FirstOrDefault();

        public static string GetClassName(this ClassDeclarationSyntax classDeclaration)
            => classDeclaration.Identifier.Text;

        public static string GetClassModifier(this ClassDeclarationSyntax classDeclaration)
            => classDeclaration.Modifiers.ToFullString().Trim();

        public static T GetAttributeValue<T>(this AttributeData attributeData, string key, T defaultValue = default)
        {
            TypedConstant? attributeRecord = attributeData?.NamedArguments
                .FirstOrDefault(c => c.Key == key)
                .Value;

            return attributeRecord is { IsNull: false } ? (T)attributeRecord.Value.Value : defaultValue;
        }

        public static bool HaveAttribute(this ClassDeclarationSyntax classDeclaration, string attributeName)
            => classDeclaration?.AttributeLists.Count > 0
               && classDeclaration
                   .AttributeLists
                   .SelectMany(SelectWithAttributes(attributeName))
                   .Any();

        public static string GetNamespace(this CompilationUnitSyntax root)
            => root.ChildNodes()
                .OfType<NamespaceDeclarationSyntax>()
                .FirstOrDefault()
                ?.Name
                .ToString();

        private static Func<AttributeListSyntax, IEnumerable<AttributeSyntax>> SelectWithAttributes(
            string attributeName)
            => l => l?.Attributes.Where(a => (a.Name as IdentifierNameSyntax)?.Identifier.Text == attributeName);

        public static IEnumerable<ITypeSymbol> GetBaseTypesAndThis(this ITypeSymbol type)
        {
            ITypeSymbol current = type;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }

        public static IEnumerable<ISymbol> GetAllMembers(this ITypeSymbol type)
            => type.GetBaseTypesAndThis().SelectMany(n => n.GetMembers());

        public static IEnumerable<IPropertySymbol> GetProperties(this INamedTypeSymbol symbol)
            => symbol.GetAllMembers()
                .Where(x => x.Kind == SymbolKind.Property)
                .OfType<IPropertySymbol>();

        public static bool Implements(this ITypeSymbol symbol, ITypeSymbol type)
            => symbol.AllInterfaces.Any(type.Equals);
    }
}
