# Console.Routing
Console.Routing is a framework that makes it easy to build command line tools. Proper command line parsing can be tricky and time consuming. This library helps you implement a command line tool with almost no overhead.

Console.Routing works with a router similar to ASP.NET. Commands and parameters from the command line are routed to a specific C# method and fills in all method parameters with the matching command line arguments.

# Documentation

## Setup
By adding two lines of code to ``Program.cs``, you enable route discovery and handling of arguments.

```csharp
 using ConsoleRouting;
 
 Routing.Handle(args);
 ```

The examples below describe commands using a fictitious tool named ``tool``.

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
Hello world!
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
Flags are used to set a switch to true. Each of the following two statements will route to method below:
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
The Flag has a default cast to bool. So you can also use a `bool` parameter. The following method has the same command line signature as above. 
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
In some tools it's common to add an argument that  comes with a flag. A good exmple of this is the equivalent of Git's commit message.

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
Notice that the `Flag<T>` has an implicit cast to `T`.

The `Flag<T>` implementation currently also supports `int`:
```powershell
> tool loop --count 5
```
```csharp   
    [Command]
    public void Loop(Flag<int> count)
    { 
    	... 
    }
```

And it supports enums, which are interpreted case insensitive:

```powershell
> tool paint -c yellow
```
```csharp   
    public enum Colors { Yellow, Blue }
    
    [Command]
    public void Paint(Flag<Color> color)
    {
    	... 
    }
```
You can check if a flag is set using:
```csharp    
    [Command]
    public void Paint(Flag<Color> color)
    {
    	if (color.IsSet) 
	...
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
    { ... }
```

## Integer parameters
Integers are also recognised as parameter types: 
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
If the user provides anything else than an integer,
in this case, the routing will not be match this method:

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

If you have the C# nullable feature enabled, you can also use that for making a parameter optional:
```csharp
    [Command]
    public void Greet(string? name)
    {
	...
    }
```

### Parameter aliases
For parameters that cannot be expressed with a csharp symbol, You can define an parameter alternative with the `[Alt]` attribute:
```csharp
> tool debug --no-color
```
```csharp
    [Command]
    public void Debug([Alt("no-color")] Flag nocolor)
    {
    
    }
```

### Parameter buckets
Some times you need a large set of optional settings. For this a regular parameter list is 
hard to read and manage. For this, you can use classes to group those parameters. This also convenient for re-use:.
You can use parameter buckets by decorating a class with a `[Bucket]` attribute.

```csharp
    [Command]
    public void Curl(CurlSettings settings)
    { 
    }

    [Bucket]
    class CurlSettings
    {
        public Flag CrlF;
        public bool Append;
        public Flag<string> Url;
        
        [Alt("use-ascii")]
        public Flag UseAscii { get; set; }
        public Flag Basic { get; set; }
        public Flag<string> RemoteName { get; set; }
    }
```

## More on commands

### Command Overloading
You can overload your commands. So if you provide two commands with the same name, but different parameter types, or different parameter count.
the proper command route will be found:
```powershell
> tool count
> tool count 3
> tool count Dracula
> tool count your luck
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
    { ...  }

    [Command]
    public void Count(string whos, string item)
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
This example (borrowed from Entity Frameowrk command line tool), can be constructed like this:
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
If you use the default implementation, you get a help command. And a help flag, that can capture any route.
With no other commands or parameters, the command list is printed to the output.
```
  > tool help
  > tool -h
```
, more detailed help is shown on the specific command
if you provide it through either:
```
  > tool help command
  > tool command -?
```
There are two ways of providing the help, with information. One is through the [Help(text)] attribute, that can be provided with each command:
```csharp
    [Command, Help("This greeting greets any provided name")]
    public void Greet(string name)
    {
        Console.WriteLine($"Hello {name}")
    }
```

The produced help text looks like this:
```
Module title:
    Help    --version | Prints this help text
    Greet   <name> | Says hello to name
```



## Documentation
By default ConsoleRouting provides a help command, that lists all the available visible commands.
But it also allows for more indepth documentation by writing
```
> tool help <command> 
```
This can optionally be followed bu subcommands.
The help can print out four segments:
1. The route, enabled by default
2. The description, provided by the `[Help( ... )]` attribute.
3. The parameter list. This one can be enriched by providing C# XML inline documentation in your code.
4. The extendeddocumentation, also provided by the `///<summary>` field in the C# XML documentation
A fully documentation enriched command will look something like this:
```csharp
        /// <summary>Says hello to the person in question</summary>
        /// <param name="name"> The name of the person that will be greeted. You can use any name in the known universe </param>
        /// <param name="uppercase">Transforms the name to all caps</param>
        /// <param name="repeats">How many times the greeting should be repeated</param>
        [Command, Help("Says hello to the given name")]
        public void Greet([Optional]string name, bool uppercase, Flag<int> repeats)
        {
            if (uppercase) name = name.ToUpper();
            for(int i = 1; i < (repeats.HasValue ? repeats.Value : 1); i++)
            {
                Console.WriteLine($"Hello {name}!");
            }
        }
```

This will produce:

```
> tool help greet
Command:
  Greet (<name>) --uppercase --repeats <value>

Description:
Says hello to the given name

Parameters:
  (<name>)             The name that will be greeted. You can use any name in the known universe
  --uppercase          Transforms the name to all caps
  --repeats <value>    How many times the greeting should be repeated

Documentation:
Says hello to the name in question
```

In order to enable xml documentation to be published with your tool - necessary for the help enrichment to work, you have to enable 
it either your build settings (Visual Studio / Project / properties / Build / Output / Xml Documentation file) or directly in the `.csproj` file of the app or dll where your commands reside:
```xml
  <PropertyGroup>
	  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  </PropertyGroup>
```


## Write your own help
You can replace the default help by providing your own implementation, and not letting the route builder discover the default.
This is the default startup 
```
  Routing.Hanlde(args);
```
In the background that runs a command that looks something like this:

```csharp
    var router = new RouterBuilder()
        .AddAssemblyOf<Program>()
	.AddDefaultHelp(this RouterBuilder builder)
        .Buid();
```
If you want to write your own implementation, you should construct a router without the default help, and write your own help method.

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
You can invoke the default help documentation by calling `PrintHelp`. You can also write your own. If you need the data from the router,
you can have the router injected inthe module like this:

```
  [Module]
  public class MyCommands
  {
  	Router router;
	
  	public MyCommands(Router router)
	{
	      	this.router = router
	}
	
	 [Command("help"), Help("Provides this help list or detailed help about a command")]
        public void Help(Arguments args = null)
        {
            if (args is null || args.Count == 0)
            {
	    	WriteMyOwnRoutes(router);
                // router.Writer.WriteRoutes(router); <- default
            }
            else
            {
	    	var result = router.Bind(args);
	    	WriteMyOwnRouteHelp(result);
                
                // router.Writer.WriteRouteHelp(result);
            }
        } 
  }
```
If you also want to capture the `-?` or `--help` flags at the end, you should implement a capturing command. See below.


# Capturing 
If you want a certain parameter to capture the route, you can add a `Capture` attribute.
If the defined capture parameter is found anywhere in a argument list, the route execution will go 
to that command, without further resolving other routes.

To have a capture, add a `Capture` attribute to a command method like this:

```csharp
    [Command, Capture("--help", "-h")]
    public void Help(Args args)
    {
        Console.WriteLine($"Help for these arguments: {args}");
    }
```

## Global Settings
You can mark any static class as a global settings class. This allows you to use the same parameters on all commands in your tool.
Only properties (not fields) will be set when a matching name is found. And currently only `bool` is supported (a flag on the command line)
But it's the plan to add `string`   and `int` (valued flags) later.

For this, use the `[Global]` flag.
```csharp
[Global]
public static class Settings
{
    public static bool Debug { get; set; }
    public static bool Verbose { get; set; }
}
```
You can have multple static classes marked as global.


# Working with Multiple Projects or Commands not in your Startup Project.
By using the default `Routing.Handle(args)` all modules and commands in your startup project will
be discovered as a route candidate. If you have commands in a different library (dll) or in multiple libraries,
you can use the RouteBuilder class instead.

The router builder allows you to add assemblies to the discovery list, either by providing the assembly 
or a type in the assembly. The example below has some duplicate functionality but it shows you
the options:

```csharp
    var router = new RouterBuilder()
        .Add(Assembly.GetExecutingAssembly())
        .AddAssemblyOf<Program>()
        .Add(AppDomain.CurrentDomain.GetAssemblies())
        .Buid();
    
    router.Handle(args);

```

# Dependency Injection
You can add services available for dependency injection through the RouterBuilder.
To add a service, use the `.AddService()` method.
```csharp
    var router = new RouterBuilder()
        .AddAssemblyOf<Program>()
        .AddService<SomeService>()
        .Buid();
	
```
Which can then be used in your command modules:
```csharp
  [Module]
  public class MyCommands
  {
     public MyCommand(SomeService service)  
     { ... }
     
     [Command]
     public Action()
     {
         service.DoWork();
     }
  }
```


