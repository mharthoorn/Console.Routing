using System.Collections.Generic;

namespace ConsoleRouting;

public class MemberDoc
{
    public string Key;
    public string Text;
 
    public Dictionary<string, string> Params = new();
}


public static class MemberDocExtensions
{
    public static string GetParamDoc(this MemberDoc doc, string name)
    {
        return doc.Params.TryGetValue(name, out var value) ? value : null;
    }
}

