using System.Collections.Generic;

namespace ConsoleRouting
{
    public static class RouterBuilderExtensions
    {
        public static RouterBuilder AddDefaultHelp(this RouterBuilder builder)
        {
            builder.AddModule<HelpModule>();
            return builder;
        }

        public static RouterBuilder AddAssemblyOf<T>(this RouterBuilder builder)
        {
            return builder.Add(typeof(T).Assembly);
        }

        public static RouterBuilder AddBinders(this RouterBuilder builder, IEnumerable<IBinder> binders)
        {
            foreach(var binder in binders) builder.AddBinder(binder);
            return builder;
        }

    }



}