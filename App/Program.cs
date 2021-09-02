using ConsoleRouting;


namespace ConsoleAppTemplate
{

    class Program
    {
        static void Main(string[] args)
        {
            //ServiceCollection collection = new();
            //collection.AddSingleton<Cow>();
            //collection.AddSingleton<Dog>();
            //var services = collection.BuildServiceProvider();
            //var method = typeof(NonService).GetMethod(nameof(NonService.Animals));

            //var instance = services.CreateInstance(method.DeclaringType);
            //method.Invoke(instance, null);

            var router = new RouterBuilder()
                .AddAssemblyOf<Program>()
                .AddDefaultHelp()
                .AddService<Cow>()
                .AddService<Dog>()
                .Build();

            router.Handle(args);
        }
    }
}
