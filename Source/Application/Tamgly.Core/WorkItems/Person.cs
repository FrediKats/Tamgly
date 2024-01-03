using System;

namespace Tamgly.Core.WorkItems;

public record Person(Guid Id, string Name)
{
    public static Person Me { get; } = new Person(Guid.Empty, "Me");

    public bool IsMe() => this == Me;
}