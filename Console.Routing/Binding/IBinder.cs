using System;

namespace ConsoleRouting
{
    public interface IBinder
    {
        bool Optional { get; }
        bool Match(Type type);
        int TryUse(Arguments arguments, Parameter param, int index, out object value);
    }

}
