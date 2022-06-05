using System;

namespace ConsoleRouting;

/// <summary>
/// A combination of a flag and a value. This replaces FlagValue (where value is string)
/// And opens up other types. 
/// </summary>
/// <typeparam name="T"></typeparam>
public class Flag<T> 
{
    public string Name;
    public T Value;

    [Obsolete("If the flag has no value, it won't parse as a valid route, so it can only have one value")]
    public bool HasValue => IsSet; // the value was set
    
    [Obsolete("Replaced by IsSet")]
    public bool HasFlag => IsSet; // the flag was provided

    public bool IsSet { get; private set; }

    public Flag(string name, T value, bool isSet = true)
    {
        this.Name = name;
        this.Value = value;
        this.IsSet = isSet;
        //this.HasFlag = hasflag;
        //this.HasValue = hasvalue;
    }

    public static Flag<T> NotGiven => new Flag<T>(null, default, false);

    //public static Flag<T> WithoutValue => new Flag<T>(null, default, true, );

    public static implicit operator T (Flag<T> flag) => flag.Value;

    public override string ToString()
    {
        return Value.ToString();
    }
    
}