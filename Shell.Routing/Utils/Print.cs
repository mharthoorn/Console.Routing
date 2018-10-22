using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shell.Routing
{
    public class Print
    {
        public static void DidYouMean(List<Route> routes)
        {
            Console.WriteLine("Did you mean:");
            foreach (var route in routes) Console.WriteLine($"  {route}");
        }


        /// <summary>
        /// Prints all (registered) routes grouped by Module. Useable as a help output or auto documentation
        /// </summary>
        public static void PrintRoutes(IEnumerable<Route> routes)
        {

            foreach (var group in routes.GroupBy(r => r.Section))
            {
                Console.WriteLine();
                Console.WriteLine($"{group.Key.Name}:");

                foreach (var route in group)
                {
                    var name = route.Method.Name.ToLower();

                    var parameters = route.ParametersDescription().Trim();
                    var description = route.Command.Description?.Trim();

                    var text = parameters;
                    if (!string.IsNullOrEmpty(parameters) && !string.IsNullOrEmpty(description)) text += " | ";
                    text += description;


                    Console.WriteLine($"  {name,-10} {text}");
                }
            }
        }
    }
}
