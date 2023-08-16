using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConsoleRouting;

[DebuggerDisplay("{Text}")]
public class Arguments : List<IArgument>
{
    List<bool> used = new();
    
    public Arguments(IEnumerable<IArgument> arguments)
    {
        foreach (var arg in arguments)
        {
            this.Add(arg);
            this.used.Add(false);
        }
    }

    public void Use(IArgument argument)
    {
        int index = this.IndexOf(argument);
        used[index] = true;
    }

    public void UseAll()
    {
        for (int i = 0; i < used.Count; i++) used[i] = true;
    }

    public bool IsUsed(IArgument argument)
    {
        int index = this.IndexOf(argument);
        return used[index];
    }
    public bool AllUsed()
    {
        foreach(bool u in used)
        {
            if (u == false) return false;
        }
        return true;
    }

    public IEnumerable<IArgument> Unused()
    {
        for (int i = 0; i < used.Count; i++) 
        {
            if (used[i] == false) yield return this[i];
        }
    }

     
    public bool TryGetCommand(int index, out string result) 
    {
        if (index < this.Count)
        {
            result = this[index].Original;
            return true;
        }
        else
        {
            
            result = null;
            return false;
        }
    }

    public bool TryGet<T>(int index, out T result) where T: IArgument
    {
        if (index < Count && this[index] is T item)
        {
            result = item;
            return true;
        }
        result = default;
        return false;
    }

    public string Text => string.Join(" ", this);
}