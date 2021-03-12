namespace ConsoleRouting
{

    /// <summary>
    /// A combination of a flag and a value. This replaces FlagValue (where value is string)
    /// And opens up other types. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Flag<T> 
    {
        public string Name;
        public T Value;
        public bool HasValue; // the value was set
        public bool HasFlag; // the flag was provided

        public Flag(string name, T value, bool hasflag = true, bool hasvalue = true)
        {
            this.Name = name;
            this.Value = value;
            this.HasFlag = hasflag;
            this.HasValue = hasvalue;
        }

        public static Flag<T> NotGiven => new Flag<T>(null, default, false, false);

        public static Flag<T> WithoutValue => new Flag<T>(null, default, true, false);

        public static implicit operator T (Flag<T> flag) => flag.Value;

        public override string ToString()
        {
            return Value.ToString();
        }
        
    }


}