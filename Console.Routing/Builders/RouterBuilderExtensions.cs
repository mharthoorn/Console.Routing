using System.Collections.Generic;
using System.Reflection;

namespace ConsoleRouting;

public static class RouterBuilderExtensions
{
    public static RouterBuilder AddDefaultHelp(this RouterBuilder builder)
    {
        builder.Discovery.AddModule<HelpModule>();
        return builder;
    }

    public static RouterBuilder AddAssembly(this RouterBuilder builder, Assembly assembly)
    {
        builder.Discovery.AddModules(assembly);
        return builder;
    }


    public static RouterBuilder AddModule<T>(this RouterBuilder builder)
    {
        builder.Discovery.AddModule<T>();
        return builder;
    }

    public static RouterBuilder AddCurrentAssembly(this RouterBuilder builder)
    {
        var assembly = Assembly.GetCallingAssembly();
        builder.AddAssembly(assembly);
        return builder;
    }

    public static RouterBuilder AddAssemblyOf(this RouterBuilder builder, object _object)
    {
        var assembly = _object.GetType().Assembly;
        builder.AddAssembly(assembly);
        return builder;
    }

    public static RouterBuilder AddAssemblyOf<T>(this RouterBuilder builder)
    {
        return builder.AddAssembly(typeof(T).Assembly);
    }

    public static RouterBuilder Add(this RouterBuilder builder, Assembly[] assemblies)
    {
        foreach (var assembly in assemblies) builder.AddAssembly(assembly);
        return builder;
    }

    public static RouterBuilder AddBinders(this RouterBuilder builder, IEnumerable<IBinder> binders)
    {
        foreach(var binder in binders) builder.AddBinder(binder);
        return builder;
    }

    public static RouterBuilder AddDefaultBinders(this RouterBuilder builder)
    {
        builder.Binders.AddDefaultBinders();
        return builder;
    }

    public static List<IBinder> AddDefaultBinders(this List<IBinder> binders)
    {
        binders.Add(new StringBinder());
        binders.Add(new EnumBinder());
        binders.Add(new IntBinder());
        binders.Add(new AssignmentBinder());
        binders.Add(new FlagValueBinder());
        binders.Add(new FlagBinder());
        binders.Add(new BoolBinder());
        binders.Add(new ArgumentsBinder());
        binders.Add(new StringArrayBinder());
        binders.Add(new PocoBinder(binders)); // recursive reference

        return binders;
    }

}