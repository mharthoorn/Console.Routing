using System;

namespace ConsoleRouting;


/// <summary>
/// Defines an alternative name for a parameter
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
public class Alt : Attribute
{
    public string Name { get; }

    public Alt(string name)
    {
        this.Name = name;
    }
}