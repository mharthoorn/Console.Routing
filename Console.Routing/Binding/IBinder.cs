using System;

namespace ConsoleRouting
{
    public interface IBinder
    {
        bool Optional { get; }
        bool Match(Type type);
        object TryUse(Arguments arguments, Parameter param, int index, ref int used);
    }

}
