using System;

namespace MMLib.ToString.Abstraction
{
    /// <summary>
    /// An attribute that indicates a class for generating ToString congestion.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ToStringAttribute : Attribute
    {
        public bool DisplayCollections { get; set; } = false;
    }
}
