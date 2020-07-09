# Console.Routing
Console.Routing is a framework that makes it easy to build command line tools. Proper command line parsing can be tricky and time consuming. This framework hopes to take that away.
With this library You can implement a command line tool with almost no overhead.

Console.Routing works with a router that somewhat similar to ASP.NET. It routes commands and parameters from the command line 
a specific C# method and fills in all method parameters with the matching command line arguments. 
If no route is found, a clear user friendly error is given. 

# Documentation

## Setup
By adding one line of code to the ``Program.Main`` you enable route discovery and handling of arguments.

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
You can expose any method to command line argument with the `[Command]` attribute. To avoid accidental exposure, 
the declaring class must be marked with `[Module]`. This example is the bare minimum to get your first command up-and-running. 

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
```powershell
> tool hello John -u
```
You can group multiple letter flags:
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
You can create this behaviour with a `Flag<string>` parameter.

```csharp   
    [Command]
    public void Log(Flag<string> message)
    {
        Logger.Log(message); 
    }
```
Notice that the Flag<string> has an implicit cast to string.

The Flag<T> implementation currently also supports `int`.
```powershell
> tool loop --count 5
```
```csharp   
    [Command]
    public void Loop(Flag<int> count)
    { ... }
```

### Assignment parameters
You can provide key value pairs (assignments) as a parameter as well:
```powershell
> tool login user=john password=secret
```
```csharp
    [Command]
    public void Login(Assignment user, Assignment password)
    { ... }
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

## More on parameters
### Optional paramters
To make a parameter optional, you can add an `[Optional]` attribute to it. The value will be set to null or default if it is not provided.
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
This attribute is not necessary on any type of flag or assignment parameter since they are optional by design.

### Parameter aliases
You can create an parameter alias with the `[Alt]` attribute:
```csharp
> tool debug --no-color
```
```csharp
    [Command]
    public void Debug([Alt("no-color")] Flag nocolor)
    {
    
    }
```

## More on commands

### Command Overloading
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
    { ... }
```

### Command name aliases
As an opposite of overloading, Console.Routing allows a single command to have multiple aliases.
```powershell
> tool greet Anna
> tool greeting Anna
```
Just like the default command names, matching is case insensitive. 
Bare in mind, that if you add an alias, you should also provide the original name in the list, if you want to make that work as well.
```csharp
    [Command("greet", "greeting")]
    public void Greet(string name)
    {
        Console.Writeline("Hello " + name);
    }
```

Aliases also allow you to use commands that C# syntax do not allow. If you must you can parse a flag as a command, but keep in mind
that it will just be treated as a literal, so if you need the abbreviation too, you have to add it yourself. 
```csharp
    [Command("info", "give-info", "--info", "-i")]
    public void Info(string name)
    {
        Console.Writeline("Hello " + name);
    }
```

## Hidden commands
If you want a command to be usable, but not showing up in the help, you can use the [Hidden] attribute:
```csharp
    [Command, Hidden]
    public void Secret()
    { ... }
```

### The Default command
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

### Nested Commands
You can create nested commands, or command groups, by marking a Module class as a command in itself:
```powershell
> tool database update
> tool database drop
```
This example (borrowed from Entity Frameowrk), can be constructed like this:
```csharp
[Module, Command]
public class Database
{
    [Command]
    public void Update() { }
    
    [Command]
    public void Drop() { }
}
```
You can create deeper nested commands by creating sub classes.
```csharp
[Module, Command]
public class Main
{
    [Command]
    public class Sub 
    {
        [Command]
        public void SubSub()
        {
        
        }
    }
}
```

### Help text
A command can have an additional [Help] attribute for descriptions in automatic documentation.
A mature command line tool, should have a help line for each command.

```csharp
    [Command, Help("This greeting greets any provided name")]
    public void Greet(string name)
    {
        Console.WriteLine($"Hello {name}")
    }
```

You can invoke the help documentation by calling `PrintHelp`:
```csharp
    [Command, Help("Prints this help text")]
    public void Help(Flag version)
    {
        Routing.PrintHelp();
    }

    [Command, Help("Says hello to name")]
    public void Greet(string name)
    { ... }
```
The produced help text looks like this:
```
My tool:
    Help    --version | Prints this help text
    Greet   <name> | Says hello to name
```
The router gives you access to all route data, if you want to write your own implementation.

## Global Settings
You can mark any static class as a global settings class. This allows you to use the same parameters on all commands in your tool.
Only properties (not fields) will be set when a matching name is found.

For this, use the `[Global]` flag.
```csharp
[Global]
public static class Settings
{
    public static bool Debug { get; set; }
}
```
You can have multple static classes marked as global.


# Working Multiple Projects or commands not in your Startup project.
By using the default `Routing.Handle(args)` all modules and commands in your startup project will
be discovered as a route candidate. If you have commands in a different library (dll) or in multiple libraries,
you can use the RouteBuilder class instead.

The route builder allows you to add assemblies to the discovery list, either by providing the assembly 
or a type in the assembly. The example below has some duplicate functionality but it shows you
the options:

```csharp
    var router = new RouteBuilder()
        .Add(Assembly.GetExecutingAssembly())
        .AddAssemblyOf<Program>()
        .Add(AppDomain.CurrentDomain.GetAssemblies())
        .Buid();
    
    router.Handle(args);

```