using ConsoleRouting;
using System.Linq;


namespace ConsoleAppTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            //Routing.Handle(args);
            //Routing.Interactive();
            var router = new RouterBuilder()
                .AddAssemblyOf<Program>()
                .AddDefaultHelp()
                .Build();
            
            router.Handle(args);
        }
    }
}
