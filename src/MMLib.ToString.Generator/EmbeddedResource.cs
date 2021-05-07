using System;
using System.IO;
using System.Reflection;

namespace MMLib.ToString.Generator
{
    internal static class EmbeddedResource
    {
        public static string GetContent(string relativePath)
        {
            string baseName = Assembly.GetExecutingAssembly().GetName().Name;
            string resourceName = relativePath
                .TrimStart('.')
                .Replace(Path.DirectorySeparatorChar, '.')
                .Replace(Path.AltDirectorySeparatorChar, '.');

            using Stream stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(baseName + "." + resourceName);

            if (stream == null)
            {
                throw new NotSupportedException();
            }

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
