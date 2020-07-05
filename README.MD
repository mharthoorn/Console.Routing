# Console.Routing
Console.Routing is a framework makes it easy to build command line tools. 
It routes command line arguments to a specific C# method, and fills in all method parameters. 
You can implement it with minimal configuration.


# Documentation

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

The examples below describe commands for a fictitious tool named ``tool``.

## Discovery
You can expose any method to command line argument with the [Command] attribute. To avoid accidental exposure, 
the declaring class must be marked with [Module]. The minimum example is the bare minimum to get your first command up-and-running. 

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
Which can be executed on the command line with:
```powershell
> tool hello
```

## Parameter types
### String parameters
To interpret a command line parameter as a string, you can add a string parameter to your command method.
```powershell
> tool hello John
```
```csharp
    [Command]
    public void Hello(string name)
    {
        Console.Writeline("Hello " + name);
    }
	
```

### Flag parameters
Flags are used to set a setting to true. 
Each of the following three command lines statements will route to this method:
```powershell
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
The Flag has a default cast to bool.

Alternatively, you can also use a bool parameter. The following method has the same command line signature as above. 
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
```powershell
> tool feed -cdr
```
```csharp
   
    [Command]
    public void Feed(Flag cat, Flag dog, Flag rabbit)
    {
    
    }
```

### Flag parameters with a value
In some tools it's common to add an argument that  comes with a flag. A good exmple of this is Git's commit message.

```powershell
> tool log --message "First commit"
> tool log -m "First commit"
```
You can create the same behaviour with a `Flag<string>` parameter.


```csharp   
    [Command]
    public void Log(Flag<string> message)
    {
        Logger.Log(message); 
    }
```
Notice that the Flag<string> has an implicit cast to string.

### Optional paramters
To make a string parameter optional, you can add an `[Optional]` attribute to it.
```powershell
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

### Assignment parameters
You can provide key value pairs (assignments) as a parameter as well:
```powershell
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
in this case, the routing will not be match this method
```powershell
> tool count 5
```
```csharp
    [Command]
    public void Count(int max)
    {
        for (int i = 0; i < count; i++) Console.WriteLine(i);
    }
```

## Command Overloading
You can overload your commands. So if you provide two commands with the same name, but different parameter types,
the proper command route will be found:
```powershell
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

## Command name aliases
As an opposite of overloading, Console.Routing allows a single command to have multiple aliases.
```powershell
> tool greet Anna
> tool greeting Anna
```
Bare in mind, that if you add an alias, you should also provide the original name in the list, if you want to make that work as well.
```csharp
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

## The Default command
You should always provide a command that respons when the user has given no input at all.
This command can also be used for root flags: if no command or sub command has been given.

```powershell
> tool --help
> tool --info
```
```csharp
    [Command, Default]
    public void Info(Flag help, Flag version)
    {
        if (help.Set) ShowHelp();
        if (version.Set) ShowVersion();
    }

```

## Help text
A command can have an additional [Help] attribute for descriptions in automatic documentation.
A mature command line tool, should have a help line for each command.

```csharp
    [Command, Help("This greeting greets any provided name")]
    public void Greet(string name)
    {
        Console.WriteLine($"Hello {name}")
	}
```


