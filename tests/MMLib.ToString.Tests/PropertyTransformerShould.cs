using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using MMLib.ToString.Generator;
using System.Linq;
using Xunit;

namespace MMLib.ToString.Tests
{
    public class PropertyTransformerShould
    {
        private const string TestClass = @"
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class TestClass
{
    public string StringProp { get; set; }
    public int IntProp { get; init; }
    public bool BoolProp { get; init; }

    public int[] ArrayProp { get; init; }
    public IEnumerable<Guid> EnumerableProp { get; set; }
    public Dictionary<string, string> DictionaryProp { get; set; }
    public ConcurrentQueue<int> ConcurrentQueueProp { get; set; }
}";

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TransformCollectionPropertiesBasedOnType(bool displayCollections)
        {
            // Arrange
            var syntaxTree = CSharpSyntaxTree.ParseText(TestClass);
            var compilation = CreateCompilation(syntaxTree);

            var type = compilation.GetTypeByMetadataName(nameof(TestClass));
            var propertyTransformer = new PropertyTransformer(compilation, displayCollections);

            var CollectionTransform = (string propertyName) => displayCollections
                ? propertyTransformer.TransformAsCollection(propertyName)
                : propertyName;


            // Act
            var transformedProperties = type.GetProperties()
                .Select(propertyTransformer.Transform)
                .ToDictionary(k => k.Key, v => v.Value);

            // Assert
            transformedProperties["StringProp"].Should().Be("StringProp");
            transformedProperties["IntProp"].Should().Be("IntProp");
            transformedProperties["BoolProp"].Should().Be("BoolProp");
            transformedProperties["ArrayProp"].Should().Be(CollectionTransform("ArrayProp"));
            transformedProperties["EnumerableProp"].Should()
                .Be(CollectionTransform("EnumerableProp"));
            transformedProperties["DictionaryProp"].Should()
                .Be(CollectionTransform("DictionaryProp"));
            transformedProperties["ConcurrentQueueProp"].Should()
                .Be(CollectionTransform("ConcurrentQueueProp"));
        }

        private static Compilation CreateCompilation(params SyntaxTree[] syntaxTrees)
            => CSharpCompilation.Create(null, syntaxTrees)
#if NETFRAMEWORK
                .WithReferences(Basic.Reference.Assemblies.Net472.References.All);
#elif NETSTANDARD
                .WithReferences(Basic.Reference.Assemblies.NetStandard20.References.All);
#elif NET
                .WithReferences(Basic.Reference.Assemblies.Net60.References.All);
#endif
    }
}
