using System.Reflection;

namespace MMLib.ToString.Generator
{
    internal record ClassModel(string Namespace, string Name, string Modifier, PropertyData[] Properties)
    {
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
    }
}
