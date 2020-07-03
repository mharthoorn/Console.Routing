# Console.Routing
Console.Routing is a framework that routes command line arguments to a specific C# method, and fills in all method parameters. 
You can implement Console.Routing it with minimal overhead.

# Setup
You only have to add one line of code to the ``Program.Main`` to enable Routing discovery and handling of arguments.
The generic parameter tells the router in which assembly (the one of the type) to find matching methods (routes).

```csharp
 class Program
 {
    static void Main(params string[] args)
    {
        Routing<Program>.Handle(args);
    }  
 }
```

# Discovery

You can bind any method with a [Command] attribute in a class with a [Module] attribute.
A command can have an additional [Help] attribute for description for automatic documenting.

Shell.Routing supports method overloading, and the following out-of-the-box C# types as parameters:
```
    string 
        any literal value 
    
    [Optional]string 
        optional parameter

    Flag version
        --version
        -s

    Assignment key
        key=value

    FlagValue message
        --message 'hello'
        -m 'hello'

```
The following attributes control discovery:
```
    [Module("One line description")]
        Any class with the Module attribute creates a group commands for the methods inside

    [Command]
    [Command("commit")]
        Any method with this attribute will become a command.

    [Default]
        In combination with the Command attribute. Allows the method to be discovered and 
        used without any command parameter. Good for default options like
        --help, --version etc.

    [Hidden]
        In combination with Command, the method will become a command, but is not listed.
    

```
 
# Examples 
The examples below describe commands for a fictitious tool named ``tool``.
```csharp
[Module("Example tools commands")]
public class ToolCommands
{
    // Allows the following usage on the command line:
    // > tool hello John 
    // > tool hello John --upper
    // > tool hello John -u
    [Command, Help(("Says Hello to the given name")]
    public void Hello(string name, Flag upper)
    {
        if (upper) name = name.ToUpper();
        Console.Writeline("Hello " + name);
    }
	
    // > tool log --message "First commit"
    // > tool log -m "First commit"
    [Command]
    public void Log(FlagValue message)
    {
        Logger.Log(message.Value);
    }
	
	
    // > tool greet
    // > tool greet John
    // > tool greeting 
    // > tool greeting John
    [Command("greet", "greeting")]
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
    [Command, Help("Logs in to the system with user/password")]
    public void Login(Assignment user, Assignment password)
    {
        //...
    }

    // > tool --help
    // > tool --info
    [Default, Hidden]
    public void Info(Flag help, Flag version)
    {
        if (help.Set) ShowHelp();
        if (version.Set) ShowVersion();
    }

}
```
