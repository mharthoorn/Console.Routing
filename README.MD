# Shell.Routing
Shell.Routing is a routing framework for command line parameters. It can be enabled with minimal overhead.
It supports method overloading and optional parameters. 
By default it binds any method with a [Command] attribute in a class with [Module] attribute.
This allows automatic documenting.

```csharp
 class Program
 {
    static void Main(params string[] args)
    {
        var router = new Router(Assembly.GetExecutingAssembly());
        var arguments = new Arguments(args);
        var result = router.Handle(arguments);
    }  
 }

 ...

[Module("tool")]
public class ToolCommands
{
    // Allows the following usage on the command line:
    // > tool name -upper
    [Command]
    public void Hello(string name, Option upper)
    {
        if (upper) name = name.ToUpper();
        Console.Writeline(name);
    }
}
```
