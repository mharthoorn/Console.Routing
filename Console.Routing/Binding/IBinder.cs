using System;

namespace ConsoleRouting
{
    public enum BindStatus
    {
        Success,
        Failed,
        NotFound, 
    }

    public interface IBinder
    {
        bool Optional { get; }
        bool Match(Type type);
        BindStatus TryUse(Arguments arguments, Parameter param, int index, ref int used, out object result);
    }

}
