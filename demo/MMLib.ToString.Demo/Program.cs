using MMLib.ToString.Abstraction;
using System;

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

    [ToString()]
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
    }

    [ToString()]
    public partial class Foo
    {
        public int Id { get; set; }

        public string Bar { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }
    }
}
