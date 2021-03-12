namespace ConsoleRouting
{
    public interface IArgument
    {
        string Original { get; }
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