# Console.Routing
Console.Routing is a framework makes it easy to build command line tools. 
It routes command line arguments to a specific C# method, and fills in all method parameters. 
You can implement it with minimal configuration.

## Setup
You only have to add one line of code to the ``Program.Main`` to enable route discovery and handling of arguments.

```csharp
 class Program
 {
    static void Main(params string[] args)
    {
        Routing.Handle(args);
    }  
 }
```

## Discovery
You can expose any method to command line argument with the [Command] attribute. The declaring class must be marked with [Module]
A command can have an additional [Help] attribute for the description in automatic documentation.

If the executable is called `tool', you can call the following method with ```tool greet John```:
```csharp
    [Command]
    public void Greet(string name)
    {
        Console.WriteLine($"Hello {name}");
    }
```

Method overloading is supported, and the following C# types can be mapped to command line arguments:
```
    string name
    // any text value 
    
    [Optional]string name
    // optional parameter

    Flag version
    // --version
    // -v

    Assignment key
    //  key=value

    FlagValue message
    // A flag with a value
    // --message 'hello'
    // -m 'hello'

```
The following attributes control discovery:
```
    [Module]
    [Module("One line description")]
        Any class with the Module attribute creates a group commands for the methods inside
        If the title is omitted, the class name is used.

    [Command]
    [Command("commit")]
        Any method in a [Module] class with this attribute will become a command. If put on a class,
        the class becomes a command group.

    [Default]
        In combination with the Command attribute. Allows the method to be discovered and 
        used without any command parameter. Good for default options like
        --help, --version etc.

    [Hidden]
        In combination with Command, the method will become a command, but is not listed.
    

```
 
## Examples 
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


    // > tool commit -m "first commit"
    [Command]
    public void Commit(FlagValue message)
    {
        
	}

}
```

## Flag abbreviation
Aligning with linux style arguments, you can abbreviate flags by using a single dash, and the first letter. 
And you can group single letter flags:

```csharp
    // feed -cdr
    [Command]
    public void Feed(Flag cat, Flag dog, Flag rabbit)
    {
    
	}
```
