using System;

namespace ConsoleRouting
{

    /// <summary>
    /// A combination of a flag and a value. This replaces FlagValue (where value is string)
    /// And opens up other types. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Option<T> 
    {
        public string Name;
        public T Value;
        public bool HasValue; // the value was set
        public bool HasFlag; // the flag was provided

        public Option(string name, T value, bool hasflag = true, bool hasvalue = true)
        {
            this.Name = name;
            this.Value = value;
            this.HasFlag = hasflag;
            this.HasValue = hasvalue;
        }

        public static Option<T> NotGiven => new Option<T>(null, default, false, false);

        public static Option<T> WithoutValue => new Option<T>(null, default, true, false);

        public static implicit operator T (Option<T> flag) => flag.Value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }


    [Obsolete("Use Flag<string>")]
    public class FlagValue : Option<string>
    {
        public FlagValue(string value) : base(null, value)
        {
        }

    }

}