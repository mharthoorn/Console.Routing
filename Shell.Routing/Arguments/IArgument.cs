namespace Shell.Routing
{
    public interface IArgument
    {
        bool Match(string name);
        string Value { get; }
    }

    public static class ArgumentExtensions
    {
        public static bool Match(this IArgument argument, Parameter parameter)
        {
            return argument.Match(parameter.Name) || argument.Match(parameter.AltName);
        }
    }


}