using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
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
            var classSymbol = classSemanticModel.GetDeclaredSymbol(syntax) as INamedTypeSymbol;

            var classModel = new ClassModel(root.GetNamespace(), syntax.GetClassName(),
                syntax.GetClassModifier(), classSymbol.GetProperties());

            string source = SourceCodeGenerator.Generate(classModel);

            return ($"{classModel.Name}-ToString.cs", source);
        }
    }
}
