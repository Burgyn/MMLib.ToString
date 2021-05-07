using Scriban;

namespace MMLib.ToString.Generator
{
    internal static class SourceCodeGenerator
    {
        public static string Generate(ClassModel model)
        {
            var template = Template.Parse(EmbeddedResource.GetContent("PartialClassTemplate.txt"));

            string output = template.Render(model, member => member.Name);

            return output;
        }
    }
}
