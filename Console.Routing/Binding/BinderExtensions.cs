using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting
{
    public static class BinderExtensions
    {
        public static IBinder FindMatch(this IEnumerable<IBinder> binders, Type type) => binders.FirstOrDefault(b => b.Match(type));
    }
}