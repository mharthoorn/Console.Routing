using System;

namespace ConsoleRouting;


[AttributeUsage(AttributeTargets.Parameter)]
public class Optional : Attribute { }


/// <summary>
/// The All attribute makes sure that the parameter will contain all arguments, but without the commands
/// This makes the parameter non consuming.
/// The parameter must be of type string[] or Arguments
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class All: Attribute { }
