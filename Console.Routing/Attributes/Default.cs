using System;

namespace ConsoleRouting;


/// <summary>
/// The Default attribute makes a command the default route if no commands are specified in the argument list.
/// </summary>
public class Default : Attribute
{

}

/// <summary>
/// The fallback attribute catches any set of arguments that cannot be matched.
/// Usually in combination with string[] or Arguments as parameters.
/// You can not do the same with Default, since default would always match any command
/// if you would combine it with these parameters.
/// </summary>
public class Fallback : Attribute { }