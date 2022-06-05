using System;
using System.Linq;

namespace ConsoleRouting;


public static class DisplayExtensions
{
    public static string GetCommandPath(this Route route)
    {
        var path = string.Join(" ", route.Nodes.Select(n => n.Names.First())).ToLower();
        return path;
    }

    public static string GetErrorMessage(this Exception exception)
    {
        if (exception.InnerException is null)
        {
            return exception.Message;
        }
        else
        {
            return GetErrorMessage(exception.InnerException);
        }

    }
}