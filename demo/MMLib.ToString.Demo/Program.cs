using MMLib.ToString.Abstraction;
using System;
using System.Collections.Generic;

namespace MMLib.ToString.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Person() { Id = 1, Name = "Nobody" };
            Console.WriteLine(p);
        }
    }

    [ToString]
    public partial class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        private string Secret { get; set; } = "Top secret";

        public Foo Foo { get; set; } = new Foo()
        {
            Id = 1,
            Bar = "bar",
            Created = DateTime.Now,
            Name = "somebody"
        };

        public List<FooBar> FooBars { get; set; } = new()
        {
            new() { Id = 1, Bar = "Bar" }
        };
    }

    [ToString(DisplayCollections = true)]
    public partial class Foo
    {
        public int Id { get; set; }

        public string Bar { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public string LastName { get; set; }
    }

    [ToString()]
    public partial class FooBar
    {
        public int Id { get; set; }

        public string Bar { get; set; }

        public int Deleted { get; set; }
    }
}
