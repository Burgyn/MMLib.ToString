# MMLib.ToString

> This is my first attempt with a [C# Source generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/).

[![Publish package](https://github.com/Burgyn/MMLib.ToString/actions/workflows/deploy.yml/badge.svg)](https://github.com/Burgyn/MMLib.ToString/actions/workflows/deploy.yml)

`dotnet add package MMLib.ToString.Generator`

We all love the new `record` type. One of the completely marginal features is that it has an override `ToString()` method.

```csharp
public record Person(int Id, string Name);

Person person = new (2, "Milan");

Console.WriteLine(person); //Output: Person {Id = 2, Name = Milan}
```

Why don't standard classes have such an output? Why do I have to implement this over and over again in each class? How many times have you override a standard `ToString` for debug purpouse?

I decided to try the source generator to allow it to generate a similar override for all classes marked with my attribute *(I want to use the Source generators for other purposes, this came to me as a nice example to try 😉)*

```csharp
[ToString()]
public partial class Person
{
    public int Id { get; set; }

    public string Name { get; set; }

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

var p = new Person() { Id = 1, Name = "Nobody" };
Console.WriteLine(p); 
//Output:
//Person {Id = 1, Name = Nobody, Foo = Foo {Id = 1, Bar = bar, Name = somebody, Created = 24. 4. 2021 20:39:04}}
```

Just do two things. Mark your class as `partial` and add my attribute `[ToString]`.
