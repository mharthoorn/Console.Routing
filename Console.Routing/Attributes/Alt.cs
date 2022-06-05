using System;

namespace ConsoleRouting;


/// <summary>
/// Defines an alternative name for a parameter
/// </summary>
public class Alt : Attribute
{
    public string Name { get; }

    public Alt(string name)
    {
        this.Name = name;
    }
}