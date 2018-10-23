# Shell.Routing
Shell.Routing is a C# / .net core framework for routing command line parameters. It can be used with minimal overhead.
It supports method overloading, flags, keyvalues, flag values and optional parameters. 

By default it binds any method with a [Command] attribute in a class with a [Module] attribute.
A command can have a description, which allows automatic documenting.

```csharp
 class Program
 {
    static void Main(params string[] args)
    {
        Routing<Program>.Handle(args);
    }  
 }

 ...

[Module("tool")]
public class ToolCommands
{
    // Allows the following usage on the command line:
    // > tool hello John 
    // > tool hello John --upper
    // > tool hello John -u
    [Command("Says Hello to the given name")]
    public void Hello(string name, Flag upper)
    {
        if (upper) name = name.ToUpper();
        Console.Writeline("Hello " + name);
    }
	
    // > tool commit --message "First commit"
    // > tool commit -m "First commit"
    [Command]
    public void Commit(FlagValue message)
    {
        if (upper) name = name.ToUpper();
        Console.Writeline("Hello " + name);
    }
	
	
    // > tool greet
    // > tool greet John
    [Command("Says hello")]
    public void Greet([Optional]string name)
    {
        if (name is null)
        {
            Console.WriteLine("Hello");
        }
        else 
        {
            Console.Writeline("Hello " + name);
        }
    }


    // > tool login user=john password=secret
    [Command("Logs in to the system with user/password")]
    public void Login(Assignment user, Assignment password)
    {
        //...
    }
}
```
