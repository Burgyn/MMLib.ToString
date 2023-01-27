using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using MMLib.ToString.Abstraction;
using System.Linq;
using System.Text;

namespace MMLib.ToString.Generator
{
    [Generator]
    public class ToStringGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ToStringReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is ToStringReceiver actorSyntaxReciver)
            {
                foreach (ClassDeclarationSyntax candidate in actorSyntaxReciver.Candidates)
                {
                    (string fileName, string sourceCode) = GeneratePartialClass(candidate, context.Compilation);

                    context.AddSource(fileName, SourceText.From(sourceCode, Encoding.UTF8));
                }
            }
        }

        private static (string fileName, string sourceCode) GeneratePartialClass(
            ClassDeclarationSyntax syntax,
            Compilation compilation)
        {
            CompilationUnitSyntax root = syntax.GetCompilationUnit();
            SemanticModel classSemanticModel = compilation.GetSemanticModel(syntax.SyntaxTree);
            var classSymbol = classSemanticModel.GetDeclaredSymbol(syntax);

            bool displayCollections = GetDisplayCollectionsValue(classSymbol);
            var propertyTransformer = new PropertyTransformer(classSemanticModel.Compilation, displayCollections);

            var properties = classSymbol.GetProperties()
                .Select(propertyTransformer.Transform)
                .ToArray();

            var classModel = new ClassModel(root.GetNamespace(), syntax.GetClassName(),
                syntax.GetClassModifier(), properties);

            string source = SourceCodeGenerator.Generate(classModel);

            return ($"{classModel.Name}-ToString.cs", source);
        }

        private static bool GetDisplayCollectionsValue(INamedTypeSymbol classSymbol)
        {
            var toStringAttribute = classSymbol.GetAttributes()
                .FirstOrDefault(c => c.AttributeClass.Name == nameof(ToStringAttribute));
            bool displayCollections =
                toStringAttribute.GetAttributeValue(nameof(ToStringAttribute.DisplayCollections), false);
            return displayCollections;
        }
    }
}
