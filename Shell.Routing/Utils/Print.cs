using System;
using System.Collections.Generic;
using System.Linq;

namespace Shell.Routing
{
    public class Print
    {
        public static void DidYouMean(List<OldRoute> routes)
        {
            Console.WriteLine("Did you mean:");
            foreach (var route in routes) Console.WriteLine($"  {route}");
        }


        /// <summary>
        /// Prints all (registered) routes grouped by Module. Useable as a help output or auto documentation
        /// </summary>
        public static void PrintRoutes(IEnumerable<OldRoute> routes)
        {

            foreach (var module in routes.GroupBy(r => r.Module))
            {
                Console.WriteLine();
                Console.WriteLine($"{module.Key.Title}:");

                foreach (var route in module)
                {
                    var parameters = route.ParametersDescription().Trim();
                    var description = route.Description?.Trim();

    
                    var text = parameters;
                    if (!string.IsNullOrEmpty(parameters) && !string.IsNullOrEmpty(description)) text += " | ";
                    text += description;


                    //Console.WriteLine($"  {route.co,-10} {text}");
                }
            }
        }
    }
}
