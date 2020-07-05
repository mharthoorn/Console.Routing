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
```csharp
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
```csharp
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

# Documentation
The examples below describe commands for a fictitious tool named ``tool``.

## Basic command
The minimum to get your first command up-and-running.
```powershell
> tool hello
```

```csharp
[Module]
public class MyTool
{
    [Command]
    public void Hello()
    {
        Console.WriteLine("Hello world!");
	}
}
```

## String parameters
```powershell
> tool hello John
```
```csharp
    [Command]
    public void Hello(string name, Flag upper)
    {
        Console.Writeline("Hello " + name);
    }
	
```

## Flags
Flags are used to set a setting to true. 
Each of the following three command lines statements will route to this method:
```
> tool hello John 
> tool hello John --upper
> tool hello John -u
```

```csharp
    [Command]
    public void Hello(string name, Flag upper)
    {
        if (upper) name = name.ToUpper();
        Console.Writeline("Hello " + name);
    }
```

Alternatively, you can also use a bool: it wll also become a flag on the command line.
```csharp
    [Command]
    public void Hello(string name, bool upper)
    {
        if (upper) name = name.ToUpper();
        Console.Writeline("Hello " + name);
    }
```
Aligning with linux style arguments, you can abbreviate flags by using a single dash, and the first letter. 
And you can group single letter flags:
```
> tool feed -cdr
```
```csharp
   
    [Command]
    public void Feed(Flag cat, Flag dog, Flag rabbit)
    {
    
    }
```

## Flags with a value
In some tools it's common to add an argument that  comes with a flag. A good exmple of this is Git's commit message:
```
> tool log --message "First commit"
> tool log -m "First commit"
```
Notice that the Flag<string> has an implicit cast to string.

```csharp   
    [Command]
    public void Log(Flag<string> message)
    {
        Logger.Log(message); 
    }
	
## Optional paramters
To make a string parameter optional, you can add an `[Optional]` attribute to it.
```
> tool greet John
```
```csharp
    [Command]
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
```

## Assignments 
You can provide key value pairs (assignments) as a parameter as well:
```
> tool login user=john password=secret
```
```csharp
    [Command]
    public void Login(Assignment user, Assignment password)
    {
        //...
    }
```

## Integer parameters
Integers are also regognised as parameter types. If the user provides anything else than an integer,
the route will not match.

> tool count 5
```
```csharp
    [Command]
    public void Count(int max)
    {
        for (int i = 0; i < count; i++) Console.WriteLine(i);
    }
```

## Overloading
You can overload your commands. So if you provide two commands with the same name, but different parameter types,
the proper command route will be found:
```
> tool count
> tool count 3
> tool count Count
```
Each of the above inputs, will route to a different method below:

```csharp
    [Command]
    public void Count()
    { ... }

    [Command]
    public void Count(int number)
    { ...  }

    [Command]
    public void Count(string name)
    {
    
	}


```

## Command name alternatives
As an opposite of overloading, Console.Routing allows a single command to have multiple aliases.
```
> tool greet Anna
> tool greeting Anna
```
    [Command("greet", "greeting")]
    public void Greet(string name)
    {
        Console.Writeline("Hello " + name);
    }


## Hidden commands
If you want a command to be usable, but not showing up in the help, you can use the [Hidden] attribute:
```csharp
    [Command, Hidden]
    public void Secret()
    {
    

	}
```

## Default command
You should always provide a command that respons when the user has given no input at all.
This command can also be used for root flags: if no command or sub command has been given.

```
> tool --help
> tool --info
```
    [Command, Default]
    public void Info(Flag help, Flag version)
    {
        if (help.Set) ShowHelp();
        if (version.Set) ShowVersion();
    }

```


